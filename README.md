# Fate.Project 
> 本框架采用的.Net Core 3.1 的框架,用的DDD的思想，实现了Redis,EFCore,MongoDB,VirtualFile,Consul,Ocelot,Mapper,AutoFac,RabbitMQ,IdentityServer4的一套分布式快速开发的框架.
## 层说明
>| 程序集| Nuget |注释 |
>| :-----:| :----:| :----: |
>| <b>Fate.Infrastructure.Consul</b> | <b>Fate.Infrastructure.Consul</b> |实现了consul的服务发现和服务注册，还有数据的仓储 |
>| <b>Fate.Infrastructure.Redis</b> | <b>Fate.Infrastructure.Redis</b> | 使用<b>StackExchange.Redis</b>实现对Redis访问层的封装，客户端支持集群的配置 |
>| <b>Fate.Infrastructure.Repository</b> | <b>Fate.Infrastructure.Repository</b> |使用EFCore来实现了数据库访问的封装，支持工作单元，仓储，分库，多上下文模式，支持一主多从配置. |
>| <b>Fate.Infrastructure.OcelotStore.EFCore</b> | <b>Fate.Infrastructure.OcelotStore.EFCore</b> |是对Ocelot的原来网关的扩展，改变原有的从文件中读取配置的方法，替换成数据库中读取配置信息，然后将数据存储到Redis缓存中 |
>| <b>Fate.Infrastructure.OcelotStore.Redis</b> | <b>Fate.Infrastructure.OcelotStore.Redis</b> |是对Ocelot的原有网关的扩展，将数据存储到Redis缓存中|
>| <b>Fate.Infrastructure.BaseRibbitMQ</b><br/><b>Fate.Infrastructure.CAP.Subscribe</b> | | 共同实现发布订阅.异步消息队列 |
>| <b>Fate.Infrastructure.Configuration</b> | | 是对Core原有的配置信息获取进行扩展，改成从远程获取 |
>| <b>Fate.Infrastructure.AutofacDependencyInjection</b> | | 使用Autofac替换原有的Unity依赖注入 |
>| <b>Fate.Infrastructure.Mapper</b> | <b>Fate.Infrastructure.Mapper</b> |  使用的AutoMapper进行实体映射 |
>| <b>Fate.Infrastructure</b> | | 基础设施层|
>| <b>Fate.Infrastructure.VirtualFile</b> | <b>Fate.Infrastructure.VirtualFile</b> | 虚拟文件系统 |
>|<b>Fate.Infrastructure.Mongo</b> | <b>Fate.Infrastructure.Mongo</b> | MongoDB仓储 |
>| <b>Fate.Infrastructure.Configuration.Management</b> | | 配置中心的面板和配置中间件提供 |
>| <b>Fate.Domain.Event</b> | | 实现领域服务的事件总线，底板使用的Redis作为数据存储，缓存订阅的事件 |
>| <b>Fate.Domain</b> | | 领域层，业务的核心层，负责需求的书写 |
>| <b>Fate.Domain.Model</b> | | 领域实体层 |
>| <b>Fate.Application</b> | | 应用层 |