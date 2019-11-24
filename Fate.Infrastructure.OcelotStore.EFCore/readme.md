>## 使用说明
>> 1. 使用的时候需要将参数中的缓存和数据库注入
>> 2. 默认的配置存储在系统内存中的缓存时间为2分钟（可自行配置）
>> 3. 当数据不存在的时候默认从数据库查询数据 追加到缓存中
>> 4. 系统重新启动的时候默认从数据库中读取数据
>> 5. <b>FileConfiguration</b>为ocelot的存储配置的实体(他的对应的持久化接口为<b>IFileConfigurationRepository</b>，如需更改持久化的方式 重新实现继承<b>IFileConfigurationRepository</b>再依赖注入即可)
>> 6. ocelot的配置信息 的持久化存储在数据库中 ，然后数据都存储在<b>OcelotConfiguration</b>数据表中，对其他表做出的更改应在最后的时候，将所有表的数据汇聚到<b>OcelotConfiguration</b>表中，因为此扩展的数据默认从<b>OcelotConfiguration</b>表中获取(此作法的原因是因为当缓存中得不到数据的话，如果再将数据汇总的话，那么将造成数据量过大时，接口的反应过长)