using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Fate.Common.Extensions;
using Fate.Common.Config;
using System.IO;
using System.Xml;

namespace Fate.Common.Infrastructure
{
    /// <summary>
    /// RSA 加密 解密
    /// </summary>
    public static class RSAHelper
    {
        /// <summary>
        /// 写入RSA内容
        /// </summary>
        public static void CreateRSAContent()
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
        public static string ToEncrypt(this string enStr)
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
        public static string ToDecrypt(this string deStr)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromXml(RSAConfig.PrivateKey);
                return Encoding.UTF8.GetString((rsa.Decrypt(Convert.FromBase64String(deStr), RSAEncryptionPadding.OaepSHA256)));
            }
        }

        /// <summary>
        /// xml格式的公钥转换未pem格式
        /// </summary>
        /// <returns></returns>
        public static string PublicXmlToPem()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(RSAConfig.PublicKey);
            return XC.RSAUtil.RsaKeyConvert.PublicKeyXmlToPem(xmlDocument.InnerXml);
        }
        /// <summary>
        /// 将pem转换成xml
        /// </summary>
        /// <param name="pem"></param>
        /// <returns></returns>
        public static string PublicKeyPemToXml(string pem)
        {
            return XC.RSAUtil.RsaKeyConvert.PublicKeyPemToXml(pem);
        }
    }
}
