>## 使用说明
>> 1. 使用此层服务，只需要 在Startup 注入 <b>RedisDependencyExtension</b>  中的扩展方法，配置redis的参数，注入的方法为单例
>> 2. 操作redis的时候 使用<b>IRedisOperationHelp</b> 接口 来进行数据的操作