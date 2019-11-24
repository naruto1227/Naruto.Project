using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Interface
{
    /// <summary>
    /// 云存储 抽象接口
    /// </summary>
    public interface ICloudStorage : ICommonSingleDependency
    {
        /// <summary>
        /// 推送文件文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        Task<string> PushFile(string fileName, Stream fileStream);
    }
}
