## 使用说明
>1.使用的时候先将<b>MongoDependencyExtension</b>中的服务注入
>```c#
            services.AddMongoServices(options =>
            {
                options.Add(new TestMongoContext() { ConnectionString = "mongodb://192.168.18.227:27018,192.168.18.227:27019,192.168.18.227:27020", DataBase = "test" });
            });
>2 操作的时候从容器中获取<b>IMongoRepository\<TMongoContext></b>接口服务,进行MongoDB仓储的操作
## 接口对象说明
>| 类 | 默认实现类 | 注释 |
>| :-----:| :----: | :----: |
>| MongoDependencyExtension |  | 依赖注入扩展 |
>| IMongoClientFactory | DefaultMongoClientFactory | <b>IMongoClient</b>工厂获取 |
>| IMongoCommand | DefaultMongoCommand | 数据的增删改接口 |
>| IMongoDataBase | DefaultMongoDataBase | 操作MongoDataBase接口 |
>| IMongoIndex | DefaultMongoIndex | 操作集合的索引 |
>| IMongoInfrastructure | MongoInfrastructure| 数据的基础设施接口 |
>| IMongoQuery | DefaultMongoQuery | 数据的查询接口 |
>| IMongoGridFS | DefaultMongoGridFS | GridFS文件操作 |
>| IMongoRepository | DefaultMongoRepository | mongo仓储的入口 |
>| MongoContext |  | mongo的上下文,每个Mongo上下文类需要继承此对象用来配置连接等参数，多租户模式只需要配置多个<b>MongoContext</b>子类 |
>| MongoContextOptions |  | 在每次上下文的请求中，存储的数据 |
>| MongoQueryableExtension |  | IMongoQueryable的扩展 |


## Mongodb 的连接实例
>mongodb://[username:password@]host1[:port1][,host2[:port2],...[,hostN[:portN]]][/[database][?options]]
>| 字段 | 说明 |
>| :-----:| :----: |
>| mongodb:// | 前缀 |
>| username:password@ |  登录数据库的用户和密码信息。 |
>| hostX:portX | 多个 mongos 的地址列表 |
>| /database| 鉴权时，用户帐号所属的数据库。 |
>| ?options | 指定额外的连接选项。 |
## 注意细节
>1.  当从mongodb中取数据的时候，如果实体类中不存在mongo中的字段，不然序列化的时候报错，mongodb中的字段必须都包含在实体类中。
>2.   mongodb读取的时候区分大小写。
>3.   当实体中不存在Id或者id或者_id 字段的时候，这个时候实体只需要继承<b>IMongoEntity</b>接口对象即可
>5. 如果需要实现读写分离在 ConnectionString 的options里添加<b>readPreference=secondaryPreferred</b>，设置读请求为Secondary节点优先
>6. 在 ConnectionString的options里添加 <b>maxPoolSize=xx</b> ，即可将客户端连接池中的连接数限制在xx以内。
>7. 如何保证数据写入到大多数节点后才返回
在 ConnectionString的options里添加 <b>w=majority</b> ，即可保证写请求成功写入大多数节点才向客户端确认。
>8. <b>*在事务中操作的时候不存在的集合需要自行创建</b>
>9. 关于使用IMongoGridFS进行文件上传的时候，当需要添加文件的附件信息时，如果需要传递一个对象信息，此对象需要继承BsonValue<b>(后期考虑去掉这个地方，自动验证传递的是否位对象，然后将其序列化)</b>