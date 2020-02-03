# <b>IdentityServer4的MongoDB的存储扩展</b>
>## 类说明
>>| 类 | 默认实现类 | 注释 |
>>| :-----:| :----: | :----: |
>>| IClientStore| ClientStore | 实现客户端信息的mongo存储工作 |
>>| IDeviceFlowStore| DeviceFlowStore | 实现设备信息的mongo存储工作 |
>>| IPersistedGrantStore| PersistedGrantStore | 实现授权信息的mongo存储工作 |
>>| IResourceStore| ResourceStore | 实现资源信息的mongo存储工作 |
>## 使用说明
>> 1. 注入的方式和之前的Id4的EF扩展一样，数据存储的扩展需要在开始注入MongoDb，详细注入规则查看<b>Fate.Infrastructure.Mongo库</b>