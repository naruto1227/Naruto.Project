using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fate.Common.OcelotStore.EFCore.DB
{
    /// <summary>
    ///  （单条记录）
    /// </summary>
    [Table("OcelotSecurityOptions")]
    public class OcelotSecurityOptions : Base.Model.IEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 父节点的id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 允许的ip （多个逗号分隔）
        /// </summary>
        public string IPAllowedList { get; set; }

        /// <summary>
        /// 禁用的ip （多个逗号分隔）
        /// </summary>
        public string IPBlockedList { get; set; }


        //public List<string> IPAllowedList { get; set; }

        //public List<string> IPBlockedList { get; set; }
    }
}
