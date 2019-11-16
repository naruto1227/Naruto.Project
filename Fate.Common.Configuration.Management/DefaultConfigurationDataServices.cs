using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fate.Common.Configuration.Management.DB;
using Fate.Common.Configuration.Management.Object;
using Fate.Common.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Fate.Common.Configuration.Management
{
    /// <summary>
    /// 配置获取的默认服务
    /// </summary>
    public class DefaultConfigurationDataServices : IConfigurationDataServices
    {
        private readonly IUnitOfWork<ConfigurationDbContent> unitOfWork;

        public DefaultConfigurationDataServices(IUnitOfWork<ConfigurationDbContent> _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task QueryDataAsync(RequestContext requestContext)
        {
            //获取传递的数据
            using (StreamReader stringReader = new StreamReader(requestContext.HttpContext.Request.Body))
            {
                var param = await stringReader.ReadToEndAsync();
                if (string.IsNullOrWhiteSpace(param))
                {
                    await WriteMessageAsync(requestContext.HttpContext, HttpStatusCode.BadRequest, "参数错误");
                    return;
                }
                //序列化
                var dto = param.ToObject<BaseQueryConfigurationDTO>();
                if (dto == null)
                {
                    await WriteMessageAsync(requestContext.HttpContext, HttpStatusCode.BadRequest, "参数错误");
                    return;
                }
                //获取数据
                var dataList = await unitOfWork.Query<ConfigurationEndPoint>()
                      .Where(a => a.EnvironmentType == dto.EnvironmentType)
                      .WhereIf(!string.IsNullOrWhiteSpace(dto.Group), a => a.Group == dto.Group).Select(a => new { a.Key, a.Value }).AsNoTracking().ToListAsync();
                //构建返回数据
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (dataList != null && dataList.Count() > 0)
                {
                    dataList.ForEach(item =>
                    {
                        dic.Add(item.Key, item.Value);
                    });
                }
                await WriteMessageAsync(requestContext.HttpContext, HttpStatusCode.OK, dic.ToJson());
            }
        }
        /// <summary>
        /// 输出消息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task WriteMessageAsync(HttpContext httpContext, HttpStatusCode statusCode, string message)
        {
            httpContext.Response.ContentType = "application/json;charset=utf-8";
            httpContext.Response.StatusCode = Convert.ToInt32(statusCode);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await httpContext.Response.Body.WriteAsync(messageBytes, 0, messageBytes.Length);
        }
    }
}
