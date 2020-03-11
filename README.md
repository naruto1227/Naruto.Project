# Fate.Project 
> 本框架采用的.Net Core 3.1 的框架,用的DDD的思想，实现了Redis,EFCore,MongoDB,VirtualFile,Consul,Ocelot,Mapper,AutoFac,RabbitMQ,IdentityServer4的一套分布式快速开发的框架.
## 层说明
>| 程序集| Nuget| 状态 |注释 |
>| :-----:| :----:|:----:| :----: |
>| <b>Fate.Infrastructure.TimerModule</b> | | 计划 | 任务调度模块 |
>| <b>Fate.Infrastructure.AspNetdentity.MongoDB</b> | | 开发 | 更改Identity的存储方式为Mongodb |
>| <b>Fate.Infrastructure.Id4.MongoDB</b> | | 维护 | 更改IdentityServer4的存储方式为Mongodb |
>| <b>Fate.Infrastructure.Id4.MongoDB.Dashboard</b> | | 计划 | 操作存储的面板 |
>| <b>Fate.Infrastructure.Consul</b> | <b>Fate.Infrastructure.Consul</b>| 维护 |实现了consul的服务发现和服务注册，还有数据的仓储 |
>| <b>Fate.Infrastructure.Redis</b> | <b>Fate.Infrastructure.Redis</b> | 维护 | 使用<b>StackExchange.Redis</b>实现对Redis访问层的封装，客户端支持集群的配置 |
>| <b>Fate.Infrastructure.Repository</b> | <b>Fate.Infrastructure.Repository</b>| 维护 |使用EFCore来实现了数据库访问的封装，支持工作单元，仓储，分库，多上下文模式，支持一主多从配置. |
>| <b>Fate.Infrastructure.OcelotStore.EFCore</b> | <b>Fate.Infrastructure.OcelotStore.EFCore</b>| 维护  |是对Ocelot的原来网关的扩展，改变原有的从文件中读取配置的方法，替换成数据库中读取配置信息 |
>| <b>Fate.Infrastructure.OcelotStore.DashBord</b> | | 计划 | ocelot操作存储的面板 |
>| <b>Fate.Infrastructure.OcelotStore.RedisProvider</b> | | 维护  | 替换IInternalConfigurationRepository接口的实现从内存操作改为从redis中操作 |
>| <b>Fate.Infrastructure.OcelotStore.Redis</b> | <b>Fate.Infrastructure.OcelotStore.Redis</b> | 维护 |是对Ocelot的原有网关的扩展，将数据存储到Redis缓存中|
>| <b>Fate.Infrastructure.BaseRibbitMQ</b><br/><b>Fate.Infrastructure.CAP.Subscribe</b> | | 维护 | 共同实现发布订阅.异步消息队列 |
>| <b>Fate.Infrastructure.Configuration</b> || 维护 | 是对Core原有的配置信息获取进行扩展，改成从远程获取 |
>| <b>Fate.Infrastructure.AutofacDependencyInjection</b> || 维护  | 使用Autofac替换原有的Unity依赖注入 |
>| <b>Fate.Infrastructure.Mapper</b> | <b>Fate.Infrastructure.Mapper</b> | 维护 |  使用的AutoMapper进行实体映射 |
>| <b>Fate.Infrastructure</b> | | 维护 | 基础设施层|
>| <b>Fate.Infrastructure.VirtualFile</b> | <b>Fate.Infrastructure.VirtualFile</b> | 维护 | 虚拟文件系统 |
>|<b>Fate.Infrastructure.Mongo</b> | <b>Fate.Infrastructure.Mongo</b>| 维护  | MongoDB仓储 |
>| <b>Fate.Infrastructure.Configuration.Management</b> | | 维护 | 配置中心的面板和配置中间件提供 |
>| <b>Fate.Domain.Event</b> | | 维护 | 实现领域服务的事件总线，底板使用的Redis作为数据存储，缓存订阅的事件 |
>| <b>Fate.Domain</b> | |  | 领域层，业务的核心层，负责需求的书写 |
>| <b>Fate.Domain.Model</b> | |  | 领域实体层 |
>| <b>Fate.Application</b> | |  | 应用层 |