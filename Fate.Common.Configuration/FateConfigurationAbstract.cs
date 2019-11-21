
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Configuration
{
    /// <summary>
    /// 张海波
    /// 2019-11-21
    /// 配置数据加载的抽象接口
    /// </summary>
    public abstract class FateConfigurationLoadAbstract
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ConcurrentDictionary<string, string>> LoadConfiguration()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            try
            {
                data = await FromHttp();
            }
            catch (Exception ex)
            {
                data = await FromFile();
                await WriteLog(ex.Message);
            }
            return data;
        }

        /// <summary>
        /// 从远程加载
        /// </summary>
        /// <returns></returns>
        protected virtual Task<ConcurrentDictionary<string, string>> FromHttp()
        {
            throw new NotImplementedException();
        }

        //文件存放的路径
        protected string filePath = Path.Combine(AppContext.BaseDirectory, "data.json");

        /// <summary>
        /// 从磁盘加载
        /// </summary>
        /// <returns></returns>
        protected virtual Task<ConcurrentDictionary<string, string>> FromFile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 推送资源到磁盘中
        /// </summary>
        /// <param name="data"></param>
        protected async Task PushFile(string data)
        {
            await Task.Run(() =>
            {
                File.WriteAllText(filePath, data);
            });
        }


        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected async Task WriteLog(string message)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "systemlog");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (FileStream fileStream = new FileStream(Path.Combine(path, DateTime.Now.ToString("yyyyMMdd") + ".log"), FileMode.Append, FileAccess.Write))
            {
                var log = Encoding.UTF8.GetBytes($"\n{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:【message】：{message}\n");
                await fileStream.WriteAsync(log, 0, log.Length);
            }
        }
    }

}
