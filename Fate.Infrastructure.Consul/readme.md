>## 使用说明 
>> 1.如需使用consul服务 ，请注入<b>ServiceCollectionExtensions</b>中的<b>AddConsul</b>扩展方法,填写配置信息即可.  
>>```c#
            services.AddConsul(options =>
            {
                options.Port = 8521;
                options.Scheme = SchemeEnum.Http;
            });
>> 2.如需在启动的时候 进行consul服务注册，紧接着使用<b>UseServiceRegister</b>方法来注入    
>> 3.此扩展实现了consul的服务发现，服务注册，数据存储，对应的接口为<b>IServiceDiscoveryManage</b>，<b>IServiceRegisterManage</b>，<b>IKVRepository</b>，如需扩展请替换接口的实现即可
## 接口对象说明
>| 类 | 默认实现类 | 注释 |
>| :-----:| :----: | :----: |
>| IConsulClientFactory | DefaultConsulClientFactory | Conusl的客户端工厂  |
>| IKVRepository | DefaultKVRepository | 访问consul的存储的仓储 |
>| IServiceDiscoveryManage | DefaultServiceDiscoveryManage | 服务发现接口 |
>| IServiceRegisterManage | DefaultServiceRegisterManage | 服务注册接口 |