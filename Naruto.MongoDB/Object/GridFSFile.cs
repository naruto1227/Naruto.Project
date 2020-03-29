using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.MongoDB.Object
{
    /// <summary>
    /// 张海波
    /// 2020-03-04
    ///GridFS文件信息
    /// </summary>
    public class GridFSFile
    {
        /// <summary>
        /// 文件的id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件的大小
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadDateTime { get; set; }

        /// <summary>
        /// 附加的信息
        /// </summary>
        public IDictionary<string, object> Metadata { get; set; }
    }
}
