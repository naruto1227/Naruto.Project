using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fate.Common.Infrastructure;
using Fate.Common.Enum;
using System.IO;
using Fate.Common.FileOperation;
using Fate.Common.Extensions;
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
        #region
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MyJsonResult> AddFile(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                myJsonResult.code = (int)MyJsonResultCodeEnum.DATACODE;
                myJsonResult.msg = "请上传文件";
            }
            else
            {
                //上传文件
                var res = await _file.AddFileAsync(file);
                //判断文件是否上传成功
                if (!res.IsNullOrEmpty())
                {
                    myJsonResult.rows = res;
                }
                else
                {
                    myJsonResult.code = (int)MyJsonResultCodeEnum.UPLOADFILECODE;
                    myJsonResult.msg = "文件上传失败";
                }
            }
            return myJsonResult;
        }

        /// <summary>
        /// 添加多个文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MyJsonResult> BulkAddFile(IFormFileCollection files)
        {
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
            return myJsonResult;
        }
        #endregion

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="files">多个文件逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MyJsonResult> DeleteFiles(string files)
        {
            if (files == null)
                throw new Common.Exceptions.MyExceptions("请填写文件的路径");
            //获取需要删除的文件信息
            var fileArray = files.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            await _file.DeleteFileAsync(fileArray);
            return myJsonResult;
        }
    }
}