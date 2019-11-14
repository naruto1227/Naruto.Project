>## 使用说明 <b>提供将ocelot的基本的数据来源从内存中扩展存储到redis缓存中（主要扩展IInternalConfigurationRepository接口的实现）,IInternalConfigurationRepository接口为每次访问获取网关的信息的接口</b>
>> 1. ocelot的基本配置的数据存储在redis缓存中，如果更新的时候需注意有个两分钟的时间差
>>2.<b>IInternalConfigurationRepository</b> 为该实体的持久化层，每次的ocelot的访问都会调用此接口获取基本配置信息（如需更改持久化重新实现<b>IInternalConfigurationRepository</b> 接口即可，然后依赖注入）
>> 3.当得到数据就将获取到的数据存储到Redis中对应的key为<b>string:Ocelot:InternalConfiguration </b>，ocelot的每次加载都会先从缓存中读取数据，如果不存在数据会先从<b>IFileConfigurationRepository</b>接口中调用<b>Get方法获取数据源</b>，之后存储到redis中（可根据自己的需求扩展）