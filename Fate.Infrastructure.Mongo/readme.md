## 接口对象说明
>| 类 | 默认实现类 | 注释 |
>| :-----:| :----: | :----: |
>| MongoDependencyExtension |  | 依赖注入扩展 |
>| IMongoClientFactory | DefaultMongoClientFactory | <b>IMongoClient</b>工厂获取 |
>| IMongoCommand | DefaultMongoCommand | 数据的增删改接口 |
>| IMongoDataBaseInfrastructure | DefaultMongoDataBaseInfrastructure | 操作MongoDataBase接口 |
>| IMongoIndexInfrastructure | DefaultMongoIndexInfrastructure | 操作集合的索引 |
>| IMongoInfrastructureBase | MongoInfrastructureBase| 数据的基础设施接口 |
>| IMongoQuery | DefaultMongoQuery | 数据的查询接口 |
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
>3.   实体中必须包含一个_id 为ObjectId的字段 (这个地方只需要所有的实体继承<b>IMongoEntity</b>对象即可)
>4. ObjectId 不可更改
>5. 如果需要实现读写分离在 ConnectionString 的options里添加<b>readPreference=secondaryPreferred</b>，设置读请求为Secondary节点优先
>6. 在 ConnectionString的options里添加 <b>maxPoolSize=xx</b> ，即可将客户端连接池中的连接数限制在xx以内。
>7. 如何保证数据写入到大多数节点后才返回
在 ConnectionString的options里添加 <b>w=majority</b> ，即可保证写请求成功写入大多数节点才向客户端确认。