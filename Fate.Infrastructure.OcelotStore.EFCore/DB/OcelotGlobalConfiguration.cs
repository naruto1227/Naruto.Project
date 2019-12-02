using Ocelot.Configuration.File;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fate.Infrastructure.OcelotStore.EFCore.DB
{
    /// <summary>
    ///  ocelot的全局配置
    /// </summary>
    [Table("OcelotGlobalConfiguration")]
    public class OcelotGlobalConfiguration : BaseRepository.Model.IEntity
    {
        /// <summary>
        ///
        /// </summary>
        public int Id { get; set; }


        public string RequestIdKey { get; set; }

        public string BaseUrl { get; set; }
        /// <summary>
        ///  下游请求的实例 （http或者https）
        /// </summary>
        public string DownstreamScheme { get; set; }


        //public FileServiceDiscoveryProvider ServiceDiscoveryProvider { get; set; }

        //public FileRateLimitOptions RateLimitOptions { get; set; }

        //public FileQoSOptions QoSOptions { get; set; }

        //public FileLoadBalancerOptions LoadBalancerOptions { get; set; }

        //public FileHttpHandlerOptions HttpHandlerOptions { get; set; }
        #region 误删
        public virtual IList<OcelotServiceDiscoveryProvider> OcelotServiceDiscoveryProvider { get; set; }

        public virtual OcelotLoadBalancer OcelotLoadBalancer { get; set; }

        public virtual OcelotQoSOptions OcelotQoSOptions { get; set; }

        public virtual OcelotRateLimitOptions OcelotRateLimitOptions { get; set; }

        public virtual OcelotHttpHandlerOptions OcelotHttpHandlerOptions { get; set; }
        #endregion
    }
}
