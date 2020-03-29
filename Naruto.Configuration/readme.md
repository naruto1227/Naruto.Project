>## 使用说明
>> 1. 提供 读取配置文件的扩展程序
>> 2.
## 接口对象说明
>| 类 | 默认实现类 | 注释 |
>| :-----:| :----: | :----: |
>| IConfigurationPublish |  | 发布消息通知订阅方,需自行实现 |
>| IFateConfigurationLoadAbstract | DefaultFateConfigurationLoad | 配置信息的加载接口，默认从远程获取配置信息，并且保存到本地，当重启的时候先从本地读取，读取不到再从远程读取 |
>| ISubscribeReloadData |  | 订阅数据重载的接口,需自行实现 |
>| FateConfigurationProvider |  | 为自定义配置的提供者 |
>| FateConfigurationSource |  | 从远程配置源加载数据到配置接口中 |
>| FateConfigurationInfrastructure |  | 用于存在自定义配置数据的key |
