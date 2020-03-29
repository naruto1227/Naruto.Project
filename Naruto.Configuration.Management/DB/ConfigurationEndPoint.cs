using Naruto.Configuration.Management.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Naruto.Configuration.Management.DB
{
    /// <summary>
    /// 张海波
    /// 2019-10-16
    /// 配置信息表
    /// </summary>
    [Table("ConfigurationEndPoint")]
    public class ConfigurationEndPoint : BaseRepository.Model.IEntity
    {

        /// <summary>
        /// 主键id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 配置的key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 配置的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 环境的类型(测试 预发 正式)
        /// </summary>
        public int EnvironmentType { get; set; } = Convert.ToInt32(EnvironmentEnum.Development);
        /// <summary>
        /// 配置所属的组名(用于区分获取同一个组的配置信息(主要用于区分不同的系统))
        /// </summary>
        public string Group { get; set; }

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
