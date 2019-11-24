using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fate.Infrastructure.Configuration.Management.DB;
using Fate.Infrastructure.Configuration.Management.Object;
using Fate.Infrastructure.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Fate.Infrastructure.Configuration.Management
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
            //获取请求参数
            BaseQueryConfigurationDTO dto = await QueryParam(requestContext);

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

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <returns></returns>
        private async Task<BaseQueryConfigurationDTO> QueryParam(RequestContext requestContext)
        {
            BaseQueryConfigurationDTO dto = default;
            //匹配请求规则
            if (requestContext.HttpContext.Request.Method == HttpMethod.Get.ToString())
            {
                dto = new BaseQueryConfigurationDTO();
                //获取请求的参数
                var queryStrings = requestContext.HttpContext.Request.Query;
                dto.Group = queryStrings["Group"].ToString();

                int.TryParse(queryStrings["EnvironmentType"].ToString(), out var environmentType);
                dto.EnvironmentType = environmentType;
            }
            else if (requestContext.HttpContext.Request.Method == HttpMethod.Post.ToString())
            {
                //获取传递的数据
                using (StreamReader stringReader = new StreamReader(requestContext.HttpContext.Request.Body))
                {
                    var param = await stringReader.ReadToEndAsync();
                    if (!string.IsNullOrWhiteSpace(param))
                        dto = param.ToObject<BaseQueryConfigurationDTO>(); //序列化
                }
            }
            return dto;
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
