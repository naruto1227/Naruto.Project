using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Fate.Common.Extensions;
using Fate.Common.Config;
using System.IO;
using System.Xml;
using Fate.Common.Redis.IRedisManage;
using Fate.Common.Interface;
namespace Fate.Common.Infrastructure
{
    /// <summary>
    /// RSA 加密 解密
    /// </summary>
    public class RSAHelper : ICommonClassSigleDependency
    {
        private readonly IRedisOperationHelp redis;
        public RSAHelper(IRedisOperationHelp redisOperationHelp)
        {
            redis = redisOperationHelp;
        }
        #region 写入文件
        /// <summary>
        /// 写入RSA内容
        /// </summary>
        public void CreateRSAPath()
        {
            using (var rsa = RSA.Create())
            {
                using (var write = new StreamWriter(RSAConfig.PublicKey))
                {
                    write.Write(rsa.ToXml(false));
                }
                using (var write = new StreamWriter(RSAConfig.PrivateKey))
                {
                    write.Write(rsa.ToXml(true));
                }
            }
        }


        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="enStr"></param>
        /// <returns></returns>
        public string ToEncryptFromPath(string enStr)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromXml(RSAConfig.PublicKey);
                return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(enStr), RSAEncryptionPadding.OaepSHA256));
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="deStr"></param>
        /// <returns></returns>
        public string ToDecryptFromPath(string deStr)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromXml(RSAConfig.PrivateKey);
                return Encoding.UTF8.GetString((rsa.Decrypt(Convert.FromBase64String(deStr), RSAEncryptionPadding.OaepSHA256)));
            }
        }
        #endregion

        #region 写入缓存
        /// <summary>
        /// 写入RSA内容
        /// </summary>
        public void CreateRSACache()
        {
            using (var rsa = RSA.Create())
            {
                redis.StringSet(RSAConfig.Cache_PublicKey, rsa.ToXml(false));
                redis.StringSet(RSAConfig.Cache_PrivateKey, rsa.ToXml(true));
            }
        }

        /// <summary>
        /// 字符串加密  
        /// </summary>
        /// <param name="enStr"></param>
        /// <returns></returns>
        public string ToEncrypt(string enStr)
        {
            using (var rsa = RSA.Create())
            {
                if (string.IsNullOrWhiteSpace(redis.StringGet(RSAConfig.Cache_PublicKey)))
                {
                    CreateRSACache();
                }
                rsa.FromXml(redis.StringGet(RSAConfig.Cache_PublicKey), false);
                //js端用 RSAEncryptionPadding 的Pkcs1
                return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(enStr), RSAEncryptionPadding.OaepSHA256));
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="deStr"></param>
        /// <returns></returns>
        public string ToDecrypt(string deStr)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromXml(redis.StringGet(RSAConfig.Cache_PrivateKey), false);
                return Encoding.UTF8.GetString((rsa.Decrypt(Convert.FromBase64String(deStr), RSAEncryptionPadding.OaepSHA256)));
            }
        }
        #endregion

        /// <summary>
        /// xml格式的公钥转换未pem格式
        /// </summary>
        /// <param name="isPath">true代表从文件中加载 false 代表从缓存加载</param>
        /// <returns></returns>
        public string PublicXmlToPem(bool isPath = false)
        {
            var xml = "";
            if (isPath)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(RSAConfig.PublicKey);
                xml = xmlDocument.InnerXml;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(redis.StringGet(RSAConfig.Cache_PublicKey)))
                {
                    CreateRSACache();
                }
                xml = redis.StringGet(RSAConfig.Cache_PublicKey);
            }

            return XC.RSAUtil.RsaKeyConvert.PublicKeyXmlToPem(xml);
        }
        /// <summary>
        /// 将pem转换成xml
        /// </summary>
        /// <param name="pem"></param>
        /// <returns></returns>
        public string PublicKeyPemToXml(string pem)
        {
            return XC.RSAUtil.RsaKeyConvert.PublicKeyPemToXml(pem);
        }
    }
}
