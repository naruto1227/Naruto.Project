using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Domain.Model.Entities
{
    /// <summary>
    /// 单号表
    /// </summary>
    public class OrderNo : IEntity
    {
        /// <summary>
        /// 单号
        /// </summary>
        public long NO { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public int Date { get; set; }
    }
}
