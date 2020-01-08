using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Configuration
{
    /// <summary>
    /// 张海波
    /// 2019-11-21
    /// 配置的默认加载对象
    /// </summary>
    public class DefaultFateConfigurationLoad : FateConfigurationLoadAbstract
    {
        /// <summary>
        /// 从文件加载
        /// </summary>
        /// <returns></returns>
        protected override Task<ConcurrentDictionary<string, string>> FromFile()
        {
            return Task.FromResult(File.ReadAllText(filePath)?.ToObject<ConcurrentDictionary<string, string>>());
        }
        /// <summary>
        /// 从远程加载
        /// </summary>
        /// <returns></returns>
        protected override async Task<ConcurrentDictionary<string, string>> FromHttp()
        {
            //从json文件中加载 获取需要访问的地址
            var configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build();
            //获取配置信息
            var configurationOptions = configuration.GetSection("ConfigurationOptions").Get<ConfigurationOptions>();
            if (configurationOptions == null)
                throw new ArgumentNullException(nameof(ConfigurationOptions));
            using (var httpClient = new HttpClient())
            {
                //网关地址
                httpClient.BaseAddress = new Uri(configurationOptions.BaseUrl);
                //超时时间
                httpClient.Timeout = TimeSpan.FromSeconds(10);

                using (HttpRequestMessage httpRequestMessag = new HttpRequestMessage())
                {
                    //请求方式
                    httpRequestMessag.Method = new HttpMethod(configurationOptions.Method);
                    //请求地址
                    httpRequestMessag.RequestUri = new Uri(httpClient.BaseAddress, configurationOptions.RequestUrl);
                    //当为post的时候 传递json数据
                    if (httpRequestMessag.Method == HttpMethod.Post)
                    {
                        httpRequestMessag.Content = new StringContent(new Dictionary<string, string>() { { "Group", configurationOptions.Group }, { "EnvironmentType", configurationOptions.EnvironmentType } }.ToJson(), Encoding.UTF8, "application/json");
                    }

                    //返回响应信息
                    using (HttpResponseMessage responseMessage = await httpClient.SendAsync(httpRequestMessag))
                    {
                        //验证状态码
                        if (responseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            var res = await responseMessage.Content.ReadAsStringAsync();
                            //如果资源存在 将资源存储再磁盘中
                            if (!string.IsNullOrWhiteSpace(res))
                            {
                                await PushFile(res).ConfigureAwait(false);
                                return res.ToObject<ConcurrentDictionary<string, string>>();
                            }
                        }
                        throw new ApplicationException(responseMessage.StatusCode.ToString());
                    }
                }
            }
        }
    }
}
