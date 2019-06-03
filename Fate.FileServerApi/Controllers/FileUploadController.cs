using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fate.Common.Infrastructure;
using Fate.Common.Enum;
using System.IO;

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
        public FileUploadController(MyJsonResult jsonResult)
        {
            myJsonResult = jsonResult;
        }
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
                //获取文件上传的根地址
                string path = StaticFieldConfig.UploadFilePath;
                //获取的时间的目录
                var time = DateTime.Now.ToString("yyyMMdd");
                if (!Directory.Exists(Path.Combine(path, time)))
                {
                    Directory.CreateDirectory(Path.Combine(path, time));
                }
                //拼接路径
                string resPath = Path.Combine(time, DateTime.Now.Ticks + file.FileName);
                //完整的路径
                path = Path.Combine(path, resPath);
                //写入文件
                using (var fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }
                //判断文件是否上传成功
                if (System.IO.File.Exists(path))
                {
                    myJsonResult.rows = resPath;
                }
                else
                {
                    myJsonResult.code = (int)MyJsonResultCodeEnum.UPLOADFILECODE;
                    myJsonResult.msg = "文件上传失败";
                }
            }
            return myJsonResult;
        }
    }
}