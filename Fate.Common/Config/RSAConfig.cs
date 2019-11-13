using Fate.Common.Extensions;
using Fate.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Fate.Common.Config
{
    /// <summary>
    /// RSA的配置中心
    /// </summary>
    public class RSAConfig
    {
        #region 用来配置存放RSA文件的地址
        /// <summary>
        /// 私钥的文件存放路径
        /// </summary>
        public static string PrivateKey
        {
            get
            {
                //获取配置的私钥的路径
                var privateKey = ConfigurationManage.GetValue("prk");
                //如果不存在就默认保存在根目录
                if (string.IsNullOrWhiteSpace(privateKey))
                {
                    //获取当前工作目录
                    var path = AppContext.BaseDirectory;
                    //获取新的地址
                    privateKey = Path.Combine(path, "PrivateKey.xml");
                }
                //判断文件是否存在
                if (!File.Exists(privateKey))
                {
                    File.Create(privateKey).Close();
                }
                return privateKey;
            }
        }

        /// <summary>
        /// 公钥的文件存放路径
        /// </summary>
        public static string PublicKey
        {
            get
            {
                //获取配置的公钥的路径
                var publicKey = ConfigurationManage.GetValue("puk");
                //如果不存在就默认保存在根目录
                if (string.IsNullOrWhiteSpace(publicKey))
                {
                    //获取当前工作目录
                    var path = AppContext.BaseDirectory;
                    //
                    path = Path.Combine(path, "wwwroot");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //获取新的地址
                    publicKey = Path.Combine(path, "PublicKey.xml");
                }
                //判断文件是否存在
                if (!File.Exists(publicKey))
                {
                    using (var rsa = RSA.Create())
                    {

                        using (var fileStream = File.Create(publicKey))
                        {
                            var data = Encoding.UTF8.GetBytes(rsa.ToXml(false));
                            fileStream.Write(data, 0, data.Length);
                        }
                        using (StreamWriter writer = new StreamWriter(PrivateKey, false))
                        {
                            writer.Write(rsa.ToXml(true));
                        }
                    }
                }
                return publicKey;
            }
        }
        #endregion

        #region 用来配置存储的RSA缓存Key
        /// <summary>
        /// 公钥
        /// </summary>
        public const string Cache_PublicKey = "publickey";
        /// <summary>
        /// 私钥
        /// </summary>
        public const string Cache_PrivateKey = "privatekey";
        #endregion
    }
}
