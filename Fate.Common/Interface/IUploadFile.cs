using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Interface
{
    /// <summary>
    /// 上传的服务
    /// </summary>
    public interface IUploadFile : ICommonSingleDependency
    {

        /// <summary>
        /// 处理单独一段本地数据上传至服务器的逻辑
        /// </summary>
        /// <param name="localFilePath">本地需要上传的文件地址</param>
        /// <param name="uploadFilePath">服务器（本地）目标地址</param>
        Task UpLoadFileFromLocal(string localFilePath, string uploadFilePath);

        /// <summary>
        /// 处理一段文件流上传至服务器的逻辑
        /// </summary>
        /// <param name="uploadFIleStream">上传的文件流</param>
        /// <param name="uploadFilePath">服务器（本地）目标地址</param>
        Task UpLoadFileFromStream(Stream uploadFIleStream, string uploadFilePath);
    }
}
