using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Infrastructure.Enum
{
    /// <summary>
    /// 返回值的code枚举
    /// </summary>
    public enum MyJsonResultEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        successCode=0,
        /// <summary>
        /// 数据异常
        /// </summary>
        dataCode=1,
        /// <summary>
        /// 文件上传失败
        /// </summary>
        uploadFailureCode = 2,
        /// <summary>
        /// 服务器错误
        /// </summary>
        serverCode=3,
        /// <summary>
        /// 验证错误
        /// </summary>
        checkCode=4,
        /// <summary>
        /// 第三方接口调用失败
        /// </summary>
        thirdError=5,
        /// <summary>
        /// 未找到资源
        /// </summary>
        noFound = 6,
    }
}
