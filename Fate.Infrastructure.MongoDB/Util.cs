using Fate.Infrastructure.MongoDB.Object;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.MongoDB
{
    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// 工具类
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 空值检查
        /// </summary>
        /// <param name="source"></param>
        public static void CheckNull(this object source)
        {
            if (source == null)
                throw new ArgumentNullException(source.GetType().Name);
        }

        public static bool IsNullOrEmpty(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ConvertJson<T>(this T val)
        {
            if (val == null)
                return default;
            return val is string ? val.ToString() : JsonConvert.SerializeObject(val);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T ConvertObj<T>(this string val)
        {
            if (val == null)
                return default;
            return JsonConvert.DeserializeObject<T>(val);
        }


        /// <summary>
        /// 将对象进行转换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static GridFSFile ConvertFile(this GridFSFileInfo source)
        {
            if (source == null)
                return default;
            return new GridFSFile
            {
                Id = source.Id.ToString(),
                FileName = source.Filename,
                Length = source.Length,
                UploadDateTime = source.UploadDateTime,
                Metadata = source.Metadata?.ToString().ConvertObj<Dictionary<string, object>>()
            };
        }
        /// <summary>
        /// 将对象进行转换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static GridFSFile ConvertFile(this GridFSFileInfo<ObjectId> source)
        {
            if (source == null)
                return default;
            return new GridFSFile
            {
                Id = source.Id.ToString(),
                FileName = source.Filename,
                Length = source.Length,
                UploadDateTime = source.UploadDateTime,
                Metadata = source.Metadata?.ToString().ConvertObj<Dictionary<string, object>>()
            };
        }
        /// <summary>
        /// 将对象进行转换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<GridFSFile> ConvertFileList(this List<GridFSFileInfo> source)
        {
            if (source == null)
                return default;
            List<GridFSFile> gridFSFiles = new List<GridFSFile>();
            source.ForEach(item =>
            {
                gridFSFiles.Add(item.ConvertFile());
            });
            return gridFSFiles;
        }

        /// <summary>
        /// 转换流
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static GridFSStream ConvertStream(this GridFSDownloadStream<ObjectId> source)
        {
            if (source == null)
                return default;
            return new GridFSStream
            {
                GridFSFile = source.FileInfo.ConvertFile(),
                Stream = source
            };
        }
    }
}
