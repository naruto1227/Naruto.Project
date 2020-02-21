>## 使用说明
>> 1. 使用此层服务，只需要 在Startup 注入 <b>RedisDependencyExtension</b>  中的扩展方法，配置redis的参数，注入的方法为单例
>>```c#
            //注入redis仓储服务
            services.AddRedisRepository(options =>
            {
                options.Connection = new string[] { "127.0.0.1:6379" };
                options.RedisPrefix = new Infrastructure.Redis.RedisConfig.RedisPrefixKey();
            });
>> 2. 操作redis的时候 使用<b>IRedisOperationHelp</b> 接口 来进行数据的操作

## 接口对象说明
>| 类 | 默认实现类 | 注释 |
>| :-----:| :----: | :----: |
>| RedisDependencyExtension |  | 依赖注入扩展 |
>| IRedisOperationHelp | DefaultRedisOperationHelp | 执行redis操作的中介者对象,也是仓储的入口 |
>| IRedisBase | DefaultRedisBase | 缓存的基类 |
>| IRedisHash | DefaultRedisHash | hash操作 |
>| IRedisKey | DefaultRedisKey | key的操作 |
>| IRedisList | DefaultRedisList | list操作 |
>| IRedisLock | DefaultRedisLock| redis锁的操作 |
>| IRedisSet | DefaultRedisSet | set操作 |
>| IRedisSortedSet | DefaultRedisSortedSet | SortedSet操作 |
>| IRedisStore | DefaultRedisStore | Store操作 |
>| IRedisString | DefaultRedisString | string 操作 |
>| IRedisSubscribe | DefaultRedisSubscribe | 发布订阅 |

## 注意细节
><b>RedisOptions</b>对象说明
>| 属性 | 类型 |注释 |
>| :-----:| :----: |:----: |
>| RedisPrefix | Object | redis的所有数据类型的key的前缀配置 |
>| Connection | string[] | redis的连接信息 |
>| DefaultDataBase | int | 默认的访问的存储库 |
>| Password | string | 密码 |
>| IsOpenSentinel | int | 是否开启哨兵 |
>| RedisSentinelIp | string[] | 哨兵地址 |
>| ConnectTimeout | int | 连接超时时间 单位毫秒 默认 300ms |
>| AsyncTimeout | int | 指定系统允许异步操作的时间，以毫秒为单位, 默认5000ms |