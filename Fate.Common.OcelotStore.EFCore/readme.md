>## 使用说明
>> 1. 使用的时候需要将参数中的缓存和数据库注入
>> 2. 默认的配置存储在系统内存中的缓存时间为2分钟（可自行配置）
>> 3. ocelot的基本配置的数据存储在redis缓存中，如果更新的时候需注意有个两分钟的时间差
>> 4. 当数据不存在的时候默认从数据库查询数据 追加到缓存中
>> 5. 系统重新启动的时候默认从数据库中读取数据
>> 6. <b>FileConfiguration</b>为ocelot的存储配置的实体(他的对应的持久化接口为<b>IFileConfigurationRepository</b>，如需更改持久化的方式 重新实现继承<b>IFileConfigurationRepository</b>再依赖注入即可)
>> 7. <b>InternalConfiguration</b>为ocelot的获取基本配置的实体 ，该实体继承与<b>IInternalConfiguration</b>，
	<b>IInternalConfigurationRepository</b> 为该实体的持久化层，每次的ocelot的访问都会调用此接口获取基本配置信息（如需更改持久化重新实现<b>IInternalConfigurationRepository</b> 接口即可，然后依赖注入）
>> 8. ocelot的配置信息 的持久化存储在数据库中 ，然后数据都存储在<b>OcelotConfiguration</b>数据表中，对其他表做出的更改应在最后的时候，将所有表的数据汇聚到<b>OcelotConfiguration</b>表中，因为此扩展的数据默认从<b>OcelotConfiguration</b>表中获取(此作法的原因是因为当缓存中得不到数据的话，如果再将数据汇总的话，那么将造成数据量过大时，接口的反应过长)，当得到数据就将获取到的数据存储到Redis中对应的key为<b>string:Ocelot:InternalConfiguration </b>，ocelot的每次加载都会先从缓存中读取数据，当读取不到的时候再去数据库中读取，之后存储到redis中（可根据自己的需求扩展）