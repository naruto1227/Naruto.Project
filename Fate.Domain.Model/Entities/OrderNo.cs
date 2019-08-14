using System;
using System.Collections.Generic;
using System.Text;
using Fate.Common.Base.Model;
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
        public long No { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public int Date { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
    }
}
