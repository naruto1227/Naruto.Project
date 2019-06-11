using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Enum
{
    /// <summary>
    /// 返回值的code枚举
    /// </summary>
    public enum MyJsonResultCodeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        SUCCESSCODE=0,
        /// <summary>
        /// 数据异常
        /// </summary>
        DATACODE=1,
        /// <summary>
        /// 文件上传失败
        /// </summary>
        UPLOADFILECODE=2,
        /// <summary>
        /// 服务器错误
        /// </summary>
        SERVERCODE=3,
        /// <summary>
        /// 验证错误
        /// </summary>
        CHECKCODE=4,
        /// <summary>
        /// 第三方接口调用失败
        /// </summary>
        thirdError=5,

    }
}
