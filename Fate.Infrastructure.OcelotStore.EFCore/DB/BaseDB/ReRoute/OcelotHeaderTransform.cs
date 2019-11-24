using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fate.Infrastructure.OcelotStore.EFCore.DB
{

    [Table("OcelotHeaderTransform")]
    /// <summary>
    /// ocelot的请求头转发
    /// </summary>
    public class OcelotHeaderTransform : Base.Model.IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 父节点的id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 改变的是上游的值 还是 下游的值 0 上游 1 下游响应
        /// </summary>
        public int IsUpOrDown { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 需要替换的值
        /// </summary>
        public string Value { get; set; }
    }
}
