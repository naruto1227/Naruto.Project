using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Fate.Infrastructure
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public class OSInfo
    {
        /// <summary>
        /// 确定当前操作系统是否为64位操作系统
        /// </summary>
        public bool Is64BitOperatingSystem { get; set; } = Environment.Is64BitOperatingSystem;
        /// <summary>
        /// 获取此本地计算机的NetBIOS名称
        /// </summary>
        public string MachineName { get; set; } = Environment.MachineName;
        /// <summary>
        /// 获取当前平台标识符和版本号
        /// </summary>
        public OperatingSystem OSVersion { get; set; } = Environment.OSVersion;
        /// <summary>
        /// 获取当前计算机上的处理器数量
        /// </summary>
        public int ProcessorCount { get; set; } = Environment.ProcessorCount;
        /// <summary>
        /// 处理器名称
        /// </summary>
        public string ProcessorName { get; set; }
        /// <summary>
        /// 获取系统目录的标准路径
        /// </summary>
        public string SystemDirectory { get; set; } = Environment.SystemDirectory;
        /// <summary>
        /// 获取操作系统的内存页面中的字节数
        /// </summary>
        public int SystemPageSize { get; set; } = Environment.SystemPageSize;
        /// <summary>
        /// 获取自系统启动以来经过的毫秒数
        /// </summary>
        public long TickCount { get; set; }
        /// <summary>
        /// 获取与当前用户关联的网络域名
        /// </summary>
        public string UserDomainName { get; set; } = Environment.UserDomainName;
        /// <summary>
        /// 获取当前登录到操作系统的用户的用户名
        /// </summary>
        public string UserName { get; set; } = Environment.UserName;
        /// <summary>
        /// 获取公共语言运行时的主要，次要，内部和修订版本号
        /// </summary>
        public Version Version { get; set; } = Environment.Version;
        /// <summary>
        /// 获取运行应用程序的.NET安装的名称
        /// </summary>
        public string FrameworkDescription { get; set; } = RuntimeInformation.FrameworkDescription;
        /// <summary>
        /// 获取描述应用程序正在运行的操作系统的字符串
        /// </summary>
        public string OSDescription { get; set; } = RuntimeInformation.OSDescription;
        /// <summary>
        /// 代表操作系统平台
        /// </summary>
        public string OS { get; set; }
        /// <summary>
        /// 总物理内存 B
        /// </summary>
        public long TotalPhysicalMemory { get; set; }
        /// <summary>
        /// 可用物理内存 B
        /// </summary>
        public long FreePhysicalMemory { get; set; }
        /// <summary>
        /// 总交换空间（Linux）B
        /// </summary>
        public long SwapTotal { get; set; }
        /// <summary>
        /// 可用交换空间（Linux）B
        /// </summary>
        public long SwapFree { get; set; }
        /// <summary>
        /// 逻辑磁盘
        /// </summary>
        public object LogicalDisk { get; set; }
    }
}