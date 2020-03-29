>## 使用说明
>> 1. 提供 配置文件从Web填写
>> 2. 使用的时候先注入服务,当前驱动层数据存储基于EFCore实现，所以需要首先将仓储服务注入（仓储的使用请查看仓储层），然后再注入配置信息
>>3. 首页的访问路径为【 <b>站点地址/Naruto/pages/index.html</b>】
>>```c#
                //注入数据库仓储服务
                services.AddRepositoryServer()
                .AddRepositoryEFOptionServer(configureOptions =>
                {
                    configureOptions.ConfigureDbContext = context => context.UseMySql("Database=ConfigurationDB;DataSource=;Port=;UserId=;Password=;Charset=utf8;");
                    configureOptions.UseEntityFramework<ConfigurationDbContent>();
                });

            services.AddControllers()
            .AddConfigurationManagement();//注入自定义的配置中心页面
## 接口对象说明
>| 类 | 默认实现类 | 注释 |
>| :-----:| :----: | :----: |
>|  |  | |