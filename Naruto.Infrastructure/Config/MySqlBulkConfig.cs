using Naruto.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Naruto.Infrastructure.Config
{
    public class MySqlBulkConfig
    {
        /// <summary>
        /// 获取 mysql批量 导入的时候 文件 存放的地址
        /// </summary>
        public static string MySqlLoadFilePath
        {
            get
            {
                //是否开启文件保护 默认开启 1 开启 0 关闭
                var isOpenSecureFilePriv = 0;
                int.TryParse(ConfigurationManage.GetValue("MySqlBulkConfig:IsOpenSecureFilePriv"), out isOpenSecureFilePriv);
                var path = "";
                //开启的时候
                if (isOpenSecureFilePriv == 1)
                {
                    //获取输入的文件的存放地址
                    var loadPath = ConfigurationManage.GetValue("MySqlBulkConfig:MySqlLoadFilePath");
                    if (string.IsNullOrWhiteSpace(loadPath))
                    {
                        throw new ArgumentNullException("mysql的批量导入的文件地址不能为空");
                    }
                    path = loadPath;
                }
                else
                {
                    path = AppContext.BaseDirectory;
                }
                return path;
            }
        }
    }
}
