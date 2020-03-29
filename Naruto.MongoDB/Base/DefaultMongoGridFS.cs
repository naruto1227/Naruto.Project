using Naruto.MongoDB.Interface;
using Naruto.MongoDB.Object;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto.MongoDB.Base
{
    /// <summary>
    /// GridFS的默认接口实现
    /// </summary>
    public class DefaultMongoGridFS<TMongoContext> : IMongoGridFS<TMongoContext> where TMongoContext : MongoContext
    {
        /// <summary>
        /// GridFS操作对象
        /// </summary>
        private readonly IMongoGridFSInfrastructure mongoGridFS;

        /// <summary>
        /// 上下文的作用域参数
        /// </summary>
        private readonly MongoContextOptions currentContextOptions;

        public DefaultMongoGridFS(IMongoGridFSInfrastructure<TMongoContext> _mongoGridFS, MongoContextOptions<TMongoContext> _currentContextOptions)
        {
            mongoGridFS = _mongoGridFS;
            currentContextOptions = _currentContextOptions;
        }
        public void DeleteById(string id)
        {
            id.CheckNull();
            mongoGridFS.Exec((gridFSBucket) =>
            {
                //验证文件是否存在
                if (Exists(id))
                {
                    //执行删除
                    gridFSBucket.Delete(ObjectId.Parse(id));
                }
            });
        }

        public async Task DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            id.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();
            await mongoGridFS.Exec(async (gridFSBucket) =>
            {
                //验证文件是否存在
                if ((await ExistsAsync(id).ConfigureAwait(false)))
                {
                    //执行删除
                    await gridFSBucket.DeleteAsync(ObjectId.Parse(id), cancellationToken).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        public byte[] DownloadAsBytesById(string id)
        {
            id.CheckNull();
            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.DownloadAsBytes(ObjectId.Parse(id)));
        }

        public Task<byte[]> DownloadAsBytesByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            id.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();
            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.DownloadAsBytesAsync(ObjectId.Parse(id), null, cancellationToken));
        }

        public byte[] DownloadAsBytesByName(string fileName)
        {
            fileName.CheckNull();
            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.DownloadAsBytesByName(fileName));
        }

        public Task<byte[]> DownloadAsBytesByNameAsync(string fileName, CancellationToken cancellationToken = default)
        {
            fileName.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();
            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.DownloadAsBytesByNameAsync(fileName, null, cancellationToken));
        }

        public void DownloadToStreamById(string id, Stream target)
        {
            id.CheckNull();
            target.CheckNull();
            mongoGridFS.Exec((gridFSBucket) => gridFSBucket.DownloadToStream(ObjectId.Parse(id), target));
        }

        public Task DownloadToStreamByIdAsync(string id, Stream target, CancellationToken cancellationToken = default)
        {
            id.CheckNull();
            target.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();
            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.DownloadToStreamAsync(ObjectId.Parse(id), target, null, cancellationToken));
        }

        public void DownloadToStreamByName(string fileName, Stream target)
        {
            fileName.CheckNull();
            target.CheckNull();
            mongoGridFS.Exec((gridFSBucket) => gridFSBucket.DownloadToStreamByName(fileName, target));
        }

        public Task DownloadToStreamByNameAsync(string fileName, Stream target, CancellationToken cancellationToken = default)
        {
            fileName.CheckNull();
            target.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();
            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.DownloadToStreamByNameAsync(fileName, target, null, cancellationToken));
        }

        public void Drop()
        {
            mongoGridFS.Exec((gridFSBucket) => gridFSBucket.Drop());
        }

        public Task DropAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.DropAsync(cancellationToken));
        }

        public GridFSFile FindById(string id)
        {
            id.CheckNull();
            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.Find(Builders<GridFSFileInfo>.Filter.Eq(a => a.IdAsBsonValue, ObjectId.Parse(id))).FirstOrDefault()).ConvertFile();
        }

        public async Task<GridFSFile> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            id.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();

            var info = await mongoGridFS.Exec((gridFSBucket) => gridFSBucket.Find(Builders<GridFSFileInfo>.Filter.Eq(a => a.IdAsBsonValue, ObjectId.Parse(id)), null, cancellationToken).FirstOrDefaultAsync().ConfigureAwait(false));

            return info.ConvertFile();
        }

        public IEnumerable<GridFSFile> FindByName(string fileName, bool isFuzzy = false)
        {
            fileName.CheckNull();
            var fitter = Builders<GridFSFileInfo>.Filter.Empty;
            if (isFuzzy)
            {
                fitter = Builders<GridFSFileInfo>.Filter.Regex(a => a.Filename, BsonRegularExpression.Create($"/{fileName}/"));
            }
            else
            {
                fitter = Builders<GridFSFileInfo>.Filter.Eq(a => a.Filename, fileName);
            }
            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.Find(fitter).ToList()).ConvertFileList();
        }

        public async Task<IEnumerable<GridFSFile>> FindByNameAsync(string fileName, bool isFuzzy = false, CancellationToken cancellationToken = default)
        {
            fileName.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();

            var fitter = Builders<GridFSFileInfo>.Filter.Empty;
            if (isFuzzy)
            {
                fitter = Builders<GridFSFileInfo>.Filter.Regex(a => a.Filename, BsonRegularExpression.Create($"/{fileName}/"));
            }
            else
            {
                fitter = Builders<GridFSFileInfo>.Filter.Eq(a => a.Filename, fileName);
            }

            return (await mongoGridFS.Exec((gridFSBucket) => gridFSBucket.Find(fitter, new GridFSFindOptions
            {
                Sort = Builders<GridFSFileInfo>.Sort.Ascending(a => a.IdAsBsonValue)
            }, cancellationToken).ToListAsync().ConfigureAwait(false))).ConvertFileList();
        }

        public GridFSStream GetDownloadStreamById(string id)
        {
            id.CheckNull();
            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.OpenDownloadStream(ObjectId.Parse(id), null)).ConvertStream();
        }

        public async Task<GridFSStream> GetDownloadStreamByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            id.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();
            return (await mongoGridFS.Exec((gridFSBucket) => gridFSBucket.OpenDownloadStreamAsync(ObjectId.Parse(id), null, cancellationToken).ConfigureAwait(false))).ConvertStream();
        }

        public GridFSStream GetDownloadStreamByName(string fileName)
        {
            fileName.CheckNull();
            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.OpenDownloadStreamByName(fileName, null)).ConvertStream();
        }

        public async Task<GridFSStream> GetDownloadStreamByNameAsync(string fileName, CancellationToken cancellationToken = default)
        {
            fileName.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await mongoGridFS.Exec((gridFSBucket) => gridFSBucket.OpenDownloadStreamByNameAsync(fileName, null, cancellationToken).ConfigureAwait(false))).ConvertStream();
        }

        public string UploadFromBytes(string filename, byte[] source, IDictionary<string, object> metadata = null)
        {
            filename.CheckNull();
            source.CheckNull();

            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.UploadFromBytes(filename, source, new GridFSUploadOptions() { Metadata = new BsonDocument(metadata ?? new Dictionary<string, object>()) })).ToString();
        }

        public async Task<string> UploadFromBytesAsync(string filename, byte[] source, IDictionary<string, object> metadata = null, CancellationToken cancellationToken = default)
        {
            filename.CheckNull();
            source.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await mongoGridFS.Exec((gridFSBucket) => gridFSBucket.UploadFromBytesAsync(filename, source, new GridFSUploadOptions() { Metadata = new BsonDocument(metadata ?? new Dictionary<string, object>()) }, cancellationToken).ConfigureAwait(false))).ToString();
        }

        public async Task<string> UploadFromStramAsync(string filename, Stream source, IDictionary<string, object> metadata = null, CancellationToken cancellationToken = default)
        {
            filename.CheckNull();
            source.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await mongoGridFS.Exec((gridFSBucket) => gridFSBucket.UploadFromStreamAsync(filename, source, new GridFSUploadOptions() { Metadata = new BsonDocument(metadata ?? new Dictionary<string, object>()) }, cancellationToken).ConfigureAwait(false))).ToString();
        }

        public string UploadFromStream(string filename, Stream source, IDictionary<string, object> metadata = null)
        {
            filename.CheckNull();
            source.CheckNull();

            return mongoGridFS.Exec((gridFSBucket) => gridFSBucket.UploadFromStream(filename, source, new GridFSUploadOptions() { Metadata = new BsonDocument(metadata ?? new Dictionary<string, object>()) })).ToString();
        }

        /// <summary>
        /// 通过id 查询文件是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(string id)
        {
            id.CheckNull();
            return mongoGridFS.Exec((gridFSBucket) =>
             {
                 return gridFSBucket.Find(Builders<GridFSFileInfo>.Filter.Eq(a => a.IdAsBsonValue, ObjectId.Parse(id))).Any();
             });
        }

        /// <summary>
        /// 通过id 查询文件是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
        {
            id.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();

            return mongoGridFS.Exec((gridFSBucket) =>
            {
                return gridFSBucket.Find(Builders<GridFSFileInfo>.Filter.Eq(a => a.IdAsBsonValue, ObjectId.Parse(id))).AnyAsync();
            });
        }

        /// <summary>
        /// 通过文件名 查询文件是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool ExistsByName(string fileName)
        {
            fileName.CheckNull();

            return mongoGridFS.Exec((gridFSBucket) =>
            {
                return gridFSBucket.Find(Builders<GridFSFileInfo>.Filter.Eq(a => a.Filename, fileName)).Any();
            });
        }

        /// <summary>
        /// 通过文件名 查询文件是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<bool> ExistsByNameAsync(string fileName, CancellationToken cancellationToken = default)
        {
            fileName.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();

            return mongoGridFS.Exec((gridFSBucket) =>
            {
                return gridFSBucket.Find(Builders<GridFSFileInfo>.Filter.Eq(a => a.Filename, fileName)).AnyAsync();
            });
        }

        /// <summary>
        /// 更改bucketname
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public void ChangeBucketName(string bucketName)
        {
            bucketName.CheckNull();
            currentContextOptions.BucketName = bucketName;
        }
        /// <summary>
        /// 更改bucketname
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public Task ChangeBucketNameAsync(string bucketName, CancellationToken cancellationToken = default)
        {
            bucketName.CheckNull();
            cancellationToken.ThrowIfCancellationRequested();

            currentContextOptions.BucketName = bucketName;
            return Task.CompletedTask;
        }
    }
}
