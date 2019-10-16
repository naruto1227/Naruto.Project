using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fate.Common.Configuration.Management.DB
{
    /// <summary>
    /// 张海波
    /// 2019-10-16
    /// 配置信息表
    /// </summary>
    [Table("ConfigurationEndPoint")]
    public class ConfigurationEndPoint : Base.Model.IEntity
    {
       
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 配置的key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 配置的值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 配置的备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
