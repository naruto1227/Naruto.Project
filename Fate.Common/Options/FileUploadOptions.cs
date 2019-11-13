using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fate.Common.Options
{
    /// <summary>
    /// 文件上传的参数配置
    /// </summary>
    public class FileUploadOptions
    {
        /// <summary>
        /// 每次写入文件中的最大的buffer的长度 单位M
        /// 默认15M
        /// </summary>
        public int BufferLength { get; set; } = 15000000;

        /// <summary>
        /// 文件的 存放地址 不填默认为根目录地址
        /// </summary>
        public string UploadFilePath { set; get; } = Path.Combine(AppContext.BaseDirectory, "UploadFile");
        /// <summary>
        /// 请求的路经头
        /// </summary>
        public PathString RequestPathName { get; set; } = new PathString("/file");
    }
}
