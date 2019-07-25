1.此层是实现RabbitMQ 接收订阅消息的层,所有的订阅消息，都在此层处理
2.此层可以引用其他层的服务
3.需要接收订阅服务的类需要继承ISubscribe接口 并在方法上加特性  [CapSubscribe("test.tt")]  (其中test.tt 为自定义的路由key， 对应发布订阅的路由key)