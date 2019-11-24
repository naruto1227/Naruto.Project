using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Fate.Infrastructure.Infrastructure;
using Fate.Infrastructure.FileOperation;
using Fate.Infrastructure.Options;
using Fate.Infrastructure.Extensions;
using Fate.Infrastructure.Enum;
using Fate.Infrastructure.Interface;

namespace Fate.Infrastructure.Middleware
{
    /// <summary>
    /// 文件上传中间件
    /// </summary>
    public class FileUploadMiddleware
    {
        private MyJsonResult myJsonResult;
        /// <summary>
        /// 文件上传的帮助类
        /// </summary>
        private IFileHelper _file;
        /// <summary>
        /// 扩展名的服务
        /// </summary>
        private FileExtensionContentTypeProvider provider;

        /// <summary>
        /// 文件上传的参数配置
        /// </summary>
        private IOptions<FileUploadOptions> options;

        /// <summary>
        /// 文件上传的返回结果
        /// </summary>
        private FileJsonResult fileResult;

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="next"></param>
        /// <param name="_myJsonResult"></param>

        public FileUploadMiddleware(RequestDelegate next, MyJsonResult _myJsonResult, IFileHelper file, FileExtensionContentTypeProvider _provider, IOptions<FileUploadOptions> _options, FileJsonResult _fileResult)
        {
            myJsonResult = _myJsonResult;
            _file = file;
            provider = _provider;
            options = _options;
            fileResult = _fileResult;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json;charset=utf-8";
            //检查当前的请求方式
            if (httpContext.Request.Method.Equals(HttpMethods.Post))
            {
                //获取上传的文件
                var files = httpContext.Request.Form.Files;
                //验证是否上传文件
                if (files == null || files.Count() <= 0)
                {
                    myJsonResult.code = (int)MyJsonResultEnum.dataCode;
                    myJsonResult.msg = "请上传文件";
                    await httpContext.Response.WriteAsync(myJsonResult.ToJson());
                    return;
                }
                var resPath = "";
                foreach (var file in files)
                {
                    //上传文件
                    var res = await _file.AddFileAsync(file);
                    //判断文件是否上传成功
                    if (!res.IsNullOrEmpty())
                    {
                        resPath += "," + res;
                    }
                }
                if (!resPath.IsNullOrEmpty())
                {
                    resPath = resPath.Substring(1, resPath.Length - 1);
                }
                fileResult.src = resPath;
                fileResult.requestName = options.Value.RequestPathName;
                myJsonResult.rows = fileResult;
            }
            else
            {
                myJsonResult.code = (int)MyJsonResultEnum.noFound;
                myJsonResult.msg = "不支持的请求方式";
            }
            await httpContext.Response.WriteAsync(myJsonResult.ToJson());
        }
    }
}
