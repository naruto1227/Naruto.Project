using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Interface
{
    public interface IFileHelper
    {
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<string> AddFileAsync(IFormFile file);
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<string> AddFileAsync(Stream fileStream, string fileName);

        /// <summary>
        /// 文件地址的删除
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        Task DeleteFileAsync(string[] files);
     }
}
