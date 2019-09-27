>## 使用说明
>> 1. 此层是实现RabbitMQ 发布消息的 服务
>> 2. 需要现在Startup中注入CAPRabbitMQServicesExtensions 中的扩展方法
>> 3. 需要实现发布消息的服务，引用此层即可
>> 4. 此层不需要继承其他层的引用
>> 5. CapRoutekey 用来填写所有的路由的key来方便维护
>> 6. CapSubscribe中的构造函数的参数填写的路由的Key,字段Group为组名，同一个路由不同的组名，都会收到消息处理。