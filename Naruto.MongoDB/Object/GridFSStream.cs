using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MongoDB.Driver.GridFS;
namespace Naruto.MongoDB.Object
{
    /// <summary>
    /// 
    /// </summary>
    public class GridFSStream
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        public GridFSFile GridFSFile { get; set; }
        /// <summary>
        /// 流的信息
        /// </summary>
        public Stream Stream { get; set; }
    }
}
