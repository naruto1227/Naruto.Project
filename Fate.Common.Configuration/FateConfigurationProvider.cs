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
        public override void Load()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            //首先从远程加载配置信息 加载失败的情况下读取备份再本地磁盘的
            try
            {
                data = FromHttp();
            }
            catch (Exception ex)
            {
                data = FromFile();
            }
            Data = data;
        }


        /// <summary>
        /// 从远程加载
        /// </summary>
        /// <returns></returns>
        private ConcurrentDictionary<string, string> FromHttp()
        {
            //从json文件中加载 获取需要访问的地址
            var configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build();
            //获取配置信息
            var configurationOptions = configuration.GetSection("ConfigurationOptions").Get<ConfigurationOptions>();
            if (configurationOptions == null)
                throw new ArgumentNullException(nameof(ConfigurationOptions));
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(configurationOptions.BaseUrl);
                httpClient.Timeout = TimeSpan.FromSeconds(45);

                HttpRequestMessage httpRequestMessag = new HttpRequestMessage();
                httpRequestMessag.Method = HttpMethod.Post;
                httpRequestMessag.RequestUri = new Uri(configurationOptions.RequestUrl);
                httpRequestMessag.Content = new StringContent(new Dictionary<string, string>() { { "Group", configurationOptions.Group }, { "EnvironmentType", configurationOptions.EnvironmentType } }.ToJson(), Encoding.UTF8, "application/json");
                //返回响应信息
                HttpResponseMessage responseMessage = httpClient.SendAsync(httpRequestMessag).GetAwaiter().GetResult();
                //验证状态码
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var res = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //如果资源存在 将资源存储再磁盘中
                    if (!string.IsNullOrWhiteSpace(res))
                    {
                        PushFile(res);
                        return res.ToObject<ConcurrentDictionary<string, string>>();
                    }
                }
                throw new ApplicationException(responseMessage.StatusCode.ToString());
            }
        }
        //文件存放的路径
        private string filePath = Path.Combine(AppContext.BaseDirectory, "data.json");
        /// <summary>
        /// 推送资源到磁盘中
        /// </summary>
        /// <param name="data"></param>
        private void PushFile(string data)
        {
            using (FileStream file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
            {
                file.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
            }
        }
        /// <summary>
        /// 从磁盘加载
        /// </summary>
        /// <returns></returns>
        private ConcurrentDictionary<string, string> FromFile()
        {
            return File.ReadAllText(filePath).ToObject<ConcurrentDictionary<string, string>>();
        }
    }
}
