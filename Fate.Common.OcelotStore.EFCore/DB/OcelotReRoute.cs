using Ocelot.Configuration.File;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fate.Common.OcelotStore.EFCore.DB
{
    [Table("OcelotReRoute")]
    /// <summary>
    /// 路由
    /// </summary>
    public class OcelotReRoute : Base.Model.IEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  上游的主机，当上游的主机为当前填写的值的时候，就会匹配到当前的 ReRoute  下的路由
        /// </summary>
        public string UpstreamHost { get; set; }

        /// <summary>
        /// 当前key主要用于请求聚合的时候 配置的路由的key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 设置上游请求的优先级 如果两个相同的路由，设置了优先级的话，将会优先匹配高的路由
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool DangerousAcceptAnyServerCertificateValidator { get; set; }

        /// <summary>
        /// 下游的请求模板
        /// </summary>
        public string DownstreamPathTemplate { get; set; }
        /// <summary>
        /// 上游的请求模板
        /// </summary>
        public string UpstreamPathTemplate { get; set; }
        /// <summary>
        /// 请求id
        /// </summary>
        public string RequestIdKey { get; set; }
        /// <summary>
        /// 标识着传递的上游的url和上游的模板的地址是否完全匹配 （例 true） 默认不区分大小写
        /// </summary>
        public bool ReRouteIsCaseSensitive { get; set; }
        /// <summary>
        /// 服务名称 用于服务发现
        /// </summary>

        public string ServiceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServiceNamespace { get; set; }
        /// <summary>
        /// 下游的请求方式 （http或者https）
        /// </summary>

        public string DownstreamScheme { get; set; }
        /// <summary>
        /// 上游的http请求方式 （多个逗号分隔）
        /// </summary>
        public string UpstreamHttpMethod { get; set; }
        //public List<string> UpstreamHttpMethod { get; set; }

        /// <summary>
        ///   (多个逗号隔开)
        /// </summary>
        public string DelegatingHandlers { get; set; }
        //public List<string> DelegatingHandlers { get; set; }


        /// <summary>
        /// 是否为服务发现 否的话需要填写 HostAndPort 主机号
        /// </summary>
        public bool IsServiceDiscovery { get; set; }

        //public Dictionary<string, string> AddHeadersToRequest { get; set; }

        //public Dictionary<string, string> UpstreamHeaderTransform { get; set; }

        //public Dictionary<string, string> DownstreamHeaderTransform { get; set; }

        //public Dictionary<string, string> AddClaimsToRequest { get; set; }

        //public Dictionary<string, string> RouteClaimsRequirement { get; set; }

        //public Dictionary<string, string> AddQueriesToRequest { get; set; }


    }
}
