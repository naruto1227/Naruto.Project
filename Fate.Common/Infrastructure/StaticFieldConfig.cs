using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Fate.Common.Infrastructure
{
    /// <summary>
    /// 此地存放静态字段
    /// </summary>
    public class StaticFieldConfig
    {
        /// <summary>
        /// 文件的上传的根地址
        /// </summary>
        public static string UploadFilePath
        {
            get
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), FileUploadPathName);
                return path;
            }
        }
        /// <summary>
        /// 文件上传保存的文件夹的名字
        /// </summary>
        public static string FileUploadPathName = "UploadFile";
        /// <summary>
        /// 接口访问文件请求的路径
        /// </summary>
        public static string FileRequestPathName = "staticfile";
    }
}
