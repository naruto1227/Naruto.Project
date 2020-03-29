using Naruto.MongoDB.Object;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto.MongoDB.Interface
{
    /// <summary>
    /// 张海波
    /// 2020-03-04
    /// 文件操作
    /// </summary>
    public interface IMongoGridFS
    {
        #region 文件上传

        /// <summary>
        /// 通过二进制上传
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="source">需要上传的资源</param>
        /// <param name="metadata">附加的数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns>文件id</returns>
        string UploadFromBytes(string filename, byte[] source, IDictionary<string, object> metadata = default);

        /// <summary>
        /// 通过流上传
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="source">需要上传的资源</param>
        /// <param name="metadata">附加的数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns>文件id</returns>
        string UploadFromStream(string filename, Stream source, IDictionary<string, object> metadata = default);


        /// <summary>
        /// 通过二进制上传
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="source">需要上传的资源</param>
        /// <param name="metadata">附加的数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns>文件id</returns>
        Task<string> UploadFromBytesAsync(string filename, byte[] source, IDictionary<string, object> metadata = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过流上传
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="source">需要上传的资源</param>
        /// <param name="metadata">附加的数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns>文件id</returns>
        Task<string> UploadFromStramAsync(string filename, Stream source, IDictionary<string, object> metadata = default, CancellationToken cancellationToken = default);

        #endregion

        #region 文件下载

        #region 同步
        /// <summary>
        /// 通过主键id下载
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        byte[] DownloadAsBytesById(string id);

        /// <summary>
        /// 通过文件名下载
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        byte[] DownloadAsBytesByName(string fileName);

        /// <summary>
        /// 下载资源到指定的流中
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="target">需要写入的流</param>
        void DownloadToStreamById(string id, Stream target);

        /// <summary>
        /// 下载资源到指定的流中
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="target">需要写入的流</param>
        void DownloadToStreamByName(string fileName, Stream target);

        /// <summary>
        /// 获取需要下载的流信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GridFSStream GetDownloadStreamById(string id);
        /// <summary>
        /// 获取需要下载的流信息
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        GridFSStream GetDownloadStreamByName(string fileName);

        #endregion

        #region 异步

        /// <summary>
        /// 通过主键id下载
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<byte[]> DownloadAsBytesByIdAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过文件名下载
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<byte[]> DownloadAsBytesByNameAsync(string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// 下载资源到指定的流中
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="target">需要写入的流</param>
        Task DownloadToStreamByIdAsync(string id, Stream target, CancellationToken cancellationToken = default);

        /// <summary>
        /// 下载资源到指定的流中
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="target">需要写入的流</param>
        Task DownloadToStreamByNameAsync(string fileName, Stream target, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取需要下载的流信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GridFSStream> GetDownloadStreamByIdAsync(string id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取需要下载的流信息
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        Task<GridFSStream> GetDownloadStreamByNameAsync(string fileName, CancellationToken cancellationToken = default);

        #endregion

        #endregion

        #region 获取文件的基本信息
        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GridFSFile FindById(string id);

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GridFSFile> FindByIdAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据文件名查询
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isFuzzy">是否进行模糊查询</param>
        /// <returns></returns>
        IEnumerable<GridFSFile> FindByName(string fileName, bool isFuzzy = false);

        /// <summary>
        /// 根据文件名查询
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isFuzzy">是否进行模糊查询</param>
        /// <returns></returns>
        Task<IEnumerable<GridFSFile>> FindByNameAsync(string fileName, bool isFuzzy = false, CancellationToken cancellationToken = default);

        #endregion

        #region 单文件删除

        /// <summary>
        /// 根据id删除文件
        /// </summary>
        /// <param name="id"></param>
        void DeleteById(string id);

        /// <summary>
        /// 根据id删除文件
        /// </summary>
        /// <param name="id"></param>
        Task DeleteByIdAsync(string id, CancellationToken cancellationToken = default);

        #endregion

        #region 删除整个bucket的文件 

        /// <summary>
        /// 清空整个bucket的文件
        /// </summary>
        void Drop();
        /// <summary>
        /// 清空整个bucket的文件
        /// </summary>
        Task DropAsync(CancellationToken cancellationToken = default);

        #endregion

        #region 验证文件是否存在

        /// <summary>
        /// 通过id 查询文件是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Exists(string id);

        /// <summary>
        /// 通过id 查询文件是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过文件名 查询文件是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        bool ExistsByName(string fileName);

        /// <summary>
        /// 通过文件名 查询文件是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<bool> ExistsByNameAsync(string fileName, CancellationToken cancellationToken = default);

        #endregion

        /// <summary>
        /// 更改bucketname
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        void ChangeBucketName(string bucketName);
        /// <summary>
        /// 更改bucketname
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task ChangeBucketNameAsync(string bucketName, CancellationToken cancellationToken = default);
    }
    /// <summary>
    /// 张海波
    /// 2020-03-04
    /// 文件操作
    /// </summary>
    public interface IMongoGridFS<TMongoContext> : IMongoGridFS where TMongoContext : MongoContext
    {

    }
}
