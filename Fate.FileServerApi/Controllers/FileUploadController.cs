using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fate.Common.Infrastructure;
using Fate.Common.Config;
using Fate.Common.Enum;
using System.IO;
using Fate.Common.FileOperation;
using Fate.Common.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using System.Diagnostics;
using Fate.Common.Exceptions;

namespace Fate.FileServerApi.Controllers
{
    /// <summary>
    /// 文件上传操作控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private MyJsonResult myJsonResult;
        private FileHelper _file;
        public FileUploadController(MyJsonResult jsonResult, FileHelper fileHelper)
        {
            myJsonResult = jsonResult;
            _file = fileHelper;
        }
        #region 新增文件
        /// <summary>
        /// 添加文件(多个文件 地址逗号分隔)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, MultipartHeadersLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        public async Task<MyJsonResult> AddFile()
        {
            //测试时间
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            IFormFileCollection files = Request.Form.Files;
            if (files == null || files.Count <= 0)
            {
                myJsonResult.code = (int)MyJsonResultCodeEnum.DATACODE;
                myJsonResult.msg = "请上传文件";
            }
            else
            {
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
                myJsonResult.rows = resPath;
            }
            //测试时间
            stopwatch.Stop();
            myJsonResult.recordCount = (int)stopwatch.ElapsedMilliseconds;
            return myJsonResult;
        }
        #endregion

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="files">多个文件逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MyJsonResult> DeleteFiles()
        {
            //获取需要删除的文件
            string files = Request.Form["files"];
            if (files == null)
                throw new Common.Exceptions.MyExceptions("请填写文件的路径");
            //获取需要删除的文件信息
            var fileArray = files.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            await _file.DeleteFileAsync(fileArray);
            return myJsonResult;
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <returns></returns>
        public IActionResult DownLoad()
        {
            //获取需要下载的文件
            var file = Request.Query["file"];
            if (!file.Any() || file.ToString().IsNullOrEmpty())
                throw new MyExceptions("请填写文件下载地址");
            //获取完整的文件地址
            var path = Path.Combine(StaticFieldConfig.UploadFilePath, file);
            if (!System.IO.File.Exists(path))
                throw new MyExceptions("需要下载的文件地址错误,找不到该文件");
            //获取文件的mime类型
            var provider = new FileExtensionContentTypeProvider();
            var contentType = "";
            provider.TryGetContentType(path, out contentType);
            return File(System.IO.File.OpenRead(path), contentType, Path.GetFileName(path));
        }
    }
}