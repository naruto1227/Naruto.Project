using Microsoft.AspNetCore.Builder;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Middleware;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Naruto.OcelotStore.EFCore
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// Ocelot 重新设置配置信息
    /// </summary>
    public class EFConfigurationProvider
    {
        /// <summary>
        /// 重新配置数据
        /// </summary>
        public static OcelotMiddlewareConfigurationDelegate Get = async builder =>
        {
            //获取服务
            //获取所有配置服务的仓储
            var fileConfigRepository = builder.ApplicationServices.GetService<IFileConfigurationRepository>();
            //获取创建配置的服务(创建一个基本的配置数据 包含路由等数据 让其将数据 传递到 IInternalConfigurationRepository 中)
            var configCreator = builder.ApplicationServices.GetService<IInternalConfigurationCreator>();
            //当前接口的作用是网关的每次调用 都会从其中获取路由的数据
            var configRepository = builder.ApplicationServices.GetService<IInternalConfigurationRepository>();

            await SetFileConfigInDataBase( fileConfigRepository, configCreator, configRepository).ConfigureAwait(false);

        };
        /// <summary>
        /// 设置存储配置信息
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fileConfigRepo"></param>
        /// <param name="internalConfigCreator"></param>
        /// <param name="internalConfigRepo"></param>
        /// <returns></returns>
        internal static async Task SetFileConfigInDataBase(
            IFileConfigurationRepository fileConfigRepo,
            IInternalConfigurationCreator internalConfigCreator, IInternalConfigurationRepository internalConfigRepo)
        {
            // 从redis中获取数据
            var fileConfigFromDataBase = await fileConfigRepo.Get().ConfigureAwait(false);

            if (IsError(fileConfigFromDataBase))
            {
                ThrowToStopOcelotStarting(fileConfigFromDataBase);
            }
            else
            {
                //设置存储
                await fileConfigRepo.Set(fileConfigFromDataBase.Data).ConfigureAwait(false);
                // create the internal config from consul data
                var internalConfig = await internalConfigCreator.Create(fileConfigFromDataBase.Data).ConfigureAwait(false);

                if (IsError(internalConfig))
                {
                    ThrowToStopOcelotStarting(internalConfig);
                }
                else
                {
                    // 
                    var response = internalConfigRepo.AddOrReplace(internalConfig.Data);

                    if (IsError(response))
                    {
                        ThrowToStopOcelotStarting(response);
                    }
                }

                if (IsError(internalConfig))
                {
                    ThrowToStopOcelotStarting(internalConfig);
                }
            }
        }
        /// <summary>
        /// 抛出错误消息
        /// </summary>
        /// <param name="config"></param>
        private static void ThrowToStopOcelotStarting(Response config)
        {
            throw new Exception($"无法启动，错误信息: {string.Join(",", config.Errors.Select(x => x.ToString()))}");
        }

        private static bool IsError(Response response)
        {
            return response == null || response.IsError;
        }
    }
}
