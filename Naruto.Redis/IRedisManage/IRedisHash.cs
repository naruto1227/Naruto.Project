using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Redis.IRedisManage
{
    /// <summary>
    /// 张海波
    /// 2019-12-6
    /// hash操作
    /// </summary>
    public interface IRedisHash : IRedisDependency
    {

        #region 同步

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashField"需要删除的字段</param>
        /// <returns></returns>
        bool Delete(string key, string hashField);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashFields"需要删除的字段</param>
        /// <returns></returns>
        long HashDelete(string key, string[] hashFields);
        /// <summary>
        /// 验证是否存在指定列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        bool Exists(string key, string hashField);
        /// <summary>
        /// 获取指定的列的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        string Get(string key, string hashField);

        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        string[] Get(string key, string[] hashFields);
        /// <summary>
        /// 获取hash的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long Length(string key);

        /// <summary>
        /// 存储hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields">存储的数据key-value结构</param>
        /// <returns></returns>
        void Add(string key, HashEntry[] hashFields);

        /// <summary>
        /// 储存单条hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        bool Add(string key, string hashField, string value);
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, string> GetAll(string key);

        /// <summary>
        /// 返回所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string[] HashValues(string key);

        #endregion

        #region 异步

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashField"需要删除的字段</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string key, string hashField);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashFields"需要删除的字段</param>
        /// <returns></returns>
        Task<long> DeleteAsync(string key, string[] hashFields);
        /// <summary>
        /// 验证是否存在指定列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key, string hashField);
        /// <summary>
        /// 获取指定的列的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        Task<string> GetAsync(string key, string hashField);

        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        Task<string[]> GetAsync(string key, string[] hashFields);
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetAllAsync(string key);
        /// <summary>
        /// 获取hash的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> LengthAsync(string key);

        /// <summary>
        /// 存储hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields">存储的数据key-value结构</param>
        /// <returns></returns>
        Task AddAsync(string key, HashEntry[] hashFields);

        /// <summary>
        /// 储存单条hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<bool> AddAsync(string key, string hashField, string value);
        /// <summary>
        /// 返回所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string[]> ValuesAsync(string key);
        #endregion
    }
}
