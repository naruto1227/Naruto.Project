drop table if exists `OcelotReRoute`;

/*==============================================================*/
/* Table: OcelotReRoute                                         */
/*==============================================================*/
create table `OcelotReRoute`
(
   Id                   int(11) not null auto_increment,
   UpstreamHost         varchar(150) comment '上游的主机，当上游的主机为当前填写的值的时候，就会匹配到当前的 ReRoute  下的路由',
   `Key`                  varchar(150) comment '当前key主要用于请求聚合的时候 配置的路由的key',
   Priority             int(11) not null comment '设置上游请求的优先级 如果两个相同的路由，设置了优先级的话，将会优先匹配高的路由',
   Timeout              int(11) not null,
   DangerousAcceptAnyServerCertificateValidator bit(1) not null,
   DownstreamPathTemplate varchar(255) comment '下游的请求模板',
   UpstreamPathTemplate varchar(150) comment '上游的请求模板',
   RequestIdKey         varchar(255) comment '请求id',
   ReRouteIsCaseSensitive bit(1) not null comment '标识着传递的上游的url和上游的模板的地址是否完全匹配 （例 true） 默认不区分大小写',
   ServiceName          varchar(150) comment '服务名称 用于服务发现',
   ServiceNamespace     varchar(150),
   DownstreamScheme     varchar(50) comment ' 下游的请求方式 （http或者https）',
   UpstreamHttpMethod   varchar(150) comment '上游的http请求方式 （多个逗号分隔）',
   DelegatingHandlers   varchar(255),
   IsServiceDiscovery   bit(1) not null comment ' 是否为服务发现 否的话需要填写 HostAndPort 主机号',
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;


drop table if exists `OcelotAggregateReRoute`;

/*==============================================================*/
/* Table: OcelotAggregateReRoute                                */
/*==============================================================*/
create table `OcelotAggregateReRoute`
(
   Id                   int(11) not null auto_increment,
   ReRouteKeys          varchar(255) comment '请求聚合的路由key  （多个逗号隔开）',
   UpstreamPathTemplate varchar(100) comment '上游请求模板',
   UpstreamHost         varchar(255) comment '上游主机',
   ReRouteIsCaseSensitive bit(1) not null comment '标识着传递的上游的url和上游的模板的地址是否完全匹配 （例 true） 默认不区分大小写',
   Aggregator           varchar(255) comment '聚合器',
   Priority             int(5) not null comment '优先级',
   UpstreamHttpMethod   varchar(100) comment '上游请求方式  当前仅支持get',
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotServiceDiscoveryProvider`;

/*==============================================================*/
/* Table: OcelotServiceDiscoveryProvider                        */
/*==============================================================*/
create table `OcelotServiceDiscoveryProvider`
(
   Id                   int(11) not null auto_increment,
	ParentId            int(11) comment '父节点的id',
   `Host`                 varchar(50) comment '服务发现的主机',
   `Port`                 int(11) not null comment '服务发现的 端口',
   Type                 varchar(50) comment '服务发现的类型 默认 使用Consul',
   Token                varchar(300),
   ConfigurationKey     varchar(255),
   PollingInterval      int(11) not null,
   Namespace            varchar(100),
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotRateLimitRule`;

/*==============================================================*/
/* Table: OcelotRateLimitRule                                   */
/*==============================================================*/
create table `OcelotRateLimitRule`
(
   Id                   int(11) not null auto_increment,
	ParentId            int(11) comment '父节点的id',
   IsReRouteOrGlobal    int(2) not null comment '当前记录是 路由节点下面的 还是 全局配置节点下的 0 路由节点 1 全局配置节点  2 自定义路由节点',
   ClientWhitelist      varchar(255) comment '客户端白名单 (多个逗号分隔)',
   EnableRateLimiting   bit(1) not null comment '是否启用限流',
   Period               varchar(150) comment '统计时间段：1s, 5m, 1h, 1d',
   PeriodTimespan       double not null comment '多少秒之后客户端可以重试',
   `Limit`              double not null comment '在统计时间段内允许的最大请求数量',
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotHttpHandlerOptions`;

/*==============================================================*/
/* Table: OcelotHttpHandlerOptions                              */
/*==============================================================*/
create table `OcelotHttpHandlerOptions`
(
   Id                   int(11) not null auto_increment,
	ParentId            int(11) comment '父节点的id',
   IsReRouteOrGlobal    int(2) not null comment '当前记录是 路由节点下面的 还是 全局配置节点下的 0 路由节点 1 全局配置节点 ',
   AllowAutoRedirect    bit(1) not null,
   UseCookieContainer   bit(1) not null,
   UseTracing           bit(1) not null,
   UseProxy             bit(1) not null,
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotQoSOptions`;

/*==============================================================*/
/* Table: OcelotQoSOptions                                      */
/*==============================================================*/
create table `OcelotQoSOptions`
(
   Id                   int(11) not null auto_increment,
	ParentId            int(11) comment '父节点的id',
   IsReRouteOrGlobal    int(2) not null comment ' 当前记录是 路由节点下面的 还是 全局配置节点下的 0 路由节点 1 全局配置节点 ',
   ExceptionsAllowedBeforeBreaking int(11) not null comment '允许多少个异常请求',
   DurationOfBreak      int(11) not null comment ' 熔断的时间，单位为秒',
   TimeoutValue         int(11) not null comment '如果下游请求的处理时间超过多少则自如将请求设置为超时',
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotConfiguration`;

/*==============================================================*/
/* Table: OcelotConfiguration                                   */
/*==============================================================*/
create table `OcelotConfiguration`
(
   Id                   varchar(255) not null,
   ReRoutes             longtext comment 'ocelot的路由',
   DynamicReRoutes      longtext comment '自定义路由',
   Aggregates           longtext comment '请求聚合',
   GlobalConfiguration  longtext comment '全局配置',
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists OcelotLoadBalancer;

/*==============================================================*/
/* Table: OcelotLoadBalancer                                    */
/*==============================================================*/
create table `OcelotLoadBalancer`
(
   Id                   int(11) not null auto_increment,
	ParentId            int(11) comment '父节点的id',
   IsReRouteOrGlobal    int(2) not null comment '当前记录是 路由节点下面的 还是 全局配置节点下的 0 路由节点 1 全局配置节点 ',
   Type                 varchar(50) comment '负载均衡 的类型',
   `Key`                  varchar(255),
   Expiry               int(11) not null,
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotGlobalConfiguration`;

/*==============================================================*/
/* Table: OcelotGlobalConfiguration                             */
/*==============================================================*/
create table `OcelotGlobalConfiguration`
(
   Id                   int(11) not null auto_increment,
   RequestIdKey         varchar(50),
   BaseUrl              varchar(255),
   DownstreamScheme     varchar(20) comment '下游请求的实例 （http或者https）',
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotAggregateReRouteConfig`;

/*==============================================================*/
/* Table: OcelotAggregateReRouteConfig                          */
/*==============================================================*/
create table `OcelotAggregateReRouteConfig`
(
   Id                   int(11) not null auto_increment,
	ParentId            int(11) comment '父节点的id',
   ReRouteKey           varchar(255) comment '路由key',
   Parameter            varchar(255),
   JsonPath             varchar(255),
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotHeaderTransform`;

/*==============================================================*/
/* Table: OcelotHeaderTransform                                 */
/*==============================================================*/
create table `OcelotHeaderTransform`
(
   Id                   int(11) not null auto_increment,
	ParentId            int(11) comment '父节点的id',
   IsUpOrDown           int(2) not null comment '改变的是上游的值 还是 下游的值 0 上游 1 下游响应',
   Header               varchar(255) comment '请求头',
   `Value`                varchar(300) comment '需要替换的值',
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotDictionaryClaims`;

/*==============================================================*/
/* Table: OcelotDictionaryClaims                                */
/*==============================================================*/
create table `OcelotDictionaryClaims`
(
   Id                   int(11) not null auto_increment,
		ParentId            int(11) comment '父节点的id',
   Type                 int(2) not null comment '存放的字典的类型  0 AddHeadersToRequest  1 AddClaimsToRequest 2 RouteClaimsRequirement 3   AddQueriesToRequest',
   `Key`                  varchar(255) comment '关键字',
   `Value`                varchar(300) comment '值',
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotAuthenticationOption`;

/*==============================================================*/
/* Table: OcelotAuthenticationOption                            */
/*==============================================================*/
create table `OcelotAuthenticationOption`
(
   Id                   int(11) not null auto_increment,
	ParentId            int(11) comment '父节点的id',
   AuthenticationProviderKey varchar(150),
   AllowedScopes        varchar(255) comment '允许的范围 多个参数逗号分隔',
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotCacheOptions`;

/*==============================================================*/
/* Table: OcelotCacheOptions                                    */
/*==============================================================*/
create table `OcelotCacheOptions`
(
   Id                   int(11) not null auto_increment,
	ParentId            int(11) comment '父节点的id',
   TtlSeconds           int(11) not null,
   Region               varchar(150),
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotSecurityOptions`;

/*==============================================================*/
/* Table: OcelotSecurityOptions                                 */
/*==============================================================*/
create table `OcelotSecurityOptions`
(
   Id                   int(11) not null auto_increment,
	ParentId            int(11) comment '父节点的id',
   IPAllowedList        longtext comment ' 允许的ip （多个逗号分隔）',
   IPBlockedList        longtext comment ' 禁用的ip （多个逗号分隔）',
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

drop table if exists `OcelotHostAndPort`;

/*==============================================================*/
/* Table: OcelotHostAndPort                                     */
/*==============================================================*/
create table `OcelotHostAndPort`
(
   Id                   int(11) not null auto_increment,
	ParentId            int(11) comment '父节点的id',
   `Host`                 varchar(50) comment '主机',
   `Port`                 int(11) not null comment '端口',
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;
