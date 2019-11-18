using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Configuration
{
    /// <summary>
    /// 张海波
    /// 2019-11-14
    /// 提供远程配置的提供者
    /// </summary>
    public class FateConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// 加载配置
        /// </summary>
        public override async void Load()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            //首先从远程加载配置信息 加载失败的情况下读取备份再本地磁盘的
            try
            {
                data = await FromHttp();
            }
            catch (Exception ex)
            {
                data = await FromFile();
                await WriteLog(ex.Message);
            }
            Data = data;
        }


        /// <summary>
        /// 从远程加载
        /// </summary>
        /// <returns></returns>
        private async Task<ConcurrentDictionary<string, string>> FromHttp()
        {
            //从json文件中加载 获取需要访问的地址
            var configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build();
            //获取配置信息
            var configurationOptions = configuration.GetSection("ConfigurationOptions").Get<ConfigurationOptions>();
            if (configurationOptions == null)
                throw new ArgumentNullException(nameof(ConfigurationOptions));
            using (var httpClient = new HttpClient())
            {
                //网关地址
                httpClient.BaseAddress = new Uri(configurationOptions.BaseUrl);
                //超时时间
                httpClient.Timeout = TimeSpan.FromSeconds(15);

                using (HttpRequestMessage httpRequestMessag = new HttpRequestMessage())
                {
                    //请求方式
                    httpRequestMessag.Method = new HttpMethod(configurationOptions.Method);
                    //请求地址
                    httpRequestMessag.RequestUri = new Uri(httpClient.BaseAddress, configurationOptions.RequestUrl);
                    //当为post的时候 传递json数据
                    if (httpRequestMessag.Method == HttpMethod.Post)
                    {
                        httpRequestMessag.Content = new StringContent(new Dictionary<string, string>() { { "Group", configurationOptions.Group }, { "EnvironmentType", configurationOptions.EnvironmentType } }.ToJson(), Encoding.UTF8, "application/json");
                    }

                    //返回响应信息
                    using (HttpResponseMessage responseMessage = await httpClient.SendAsync(httpRequestMessag))
                    {
                        //验证状态码
                        if (responseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            var res = await responseMessage.Content.ReadAsStringAsync();
                            //如果资源存在 将资源存储再磁盘中
                            if (!string.IsNullOrWhiteSpace(res))
                            {
                                await PushFile(res);
                                return res.ToObject<ConcurrentDictionary<string, string>>();
                            }
                        }
                        throw new ApplicationException(responseMessage.StatusCode.ToString());
                    }
                }
            }
        }
        //文件存放的路径
        private string filePath = Path.Combine(AppContext.BaseDirectory, "data.json");
        /// <summary>
        /// 推送资源到磁盘中
        /// </summary>
        /// <param name="data"></param>
        private async Task PushFile(string data)
        {
            await Task.Run(() =>
             {
                 File.WriteAllText(filePath, data);
             });
        }
        /// <summary>
        /// 从磁盘加载
        /// </summary>
        /// <returns></returns>
        private async Task<ConcurrentDictionary<string, string>> FromFile()
        {
            return await Task.Run(() =>
             {
                 return File.ReadAllText(filePath)?.ToObject<ConcurrentDictionary<string, string>>();
             });
        }
        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task WriteLog(string message)
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
