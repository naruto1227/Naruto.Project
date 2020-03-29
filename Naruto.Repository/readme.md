## 接口对象说明
>| 类 | 默认实现类 | 注释 |
>| :-----:| :----: | :----: |
>| IUnitOfWork<> | UnitOfWork<> | <b>整个仓储的访问入口,可以从此获取对EFCore的操作和ADO的操作，单上下文的事务的操作</b> |
>| IUnitOfWorkTran | UnitOfWorkTran | 对于多工作单元模式的时候(多上下文模式)，可以批量的操作事务,此接口是对<b>IUnitOfWork<></b>接口的封装 |
>| IDbContextFactory | DbContextFactory | 上下文工厂，获取所有注入的上下文的信息，并可以获取对应的主库和从库的上下文信息 |
>| ISlaveDbConnectionFactory | DefaultSlaveDbConnectionFactory | 当前接口的作用为了实现在注入的时候，当开启主从模式的时候自动从从库中获取数据库的ip和port，以此来进行后台服务的心跳检查的操作 |
>| IRepositoryCommand<,> | RepositoryCommand<,> |  仓储的增删改的 接口层 |
>| IRepositoryQuery | RepositoryQueryAbstract | 仓库的查询的抽象接口 |
>| IRepositoryMasterQuery<,> | RepositoryMasterQuery<,> | 仓储的主库查询 |
>| IRepositoryQuery<,> | RepositoryQuery<,> | 仓储的从库查询接口 |
>| ISqlCommand<> | SqlCommand<> | ADO的增删改接口 |
>| ISqlQuery | SqlQueryAbstract | ADO的查询抽象接口 |
>| ISqlQuery<> | SqlQuery<> | 执行sql语句的从库查询操作接口 |
>| ISqlMasterQuery<> | SqlMasterQuery<> | 执行sql语句的主库查询操作接口 |
>| IRepositoryWriteInfrastructure<> | RepositoryWriteInfrastructure<> | 用来执行仓储的读写的委托方法 |
>| IRepositoryReadInfrastructure<> | RepositoryReadInfrastructure<> | 用来执行仓储的读的委托方法 |
>| IRepositoryInfrastructureBase<> | RepositoryInfrastructureBase<> | 基础设施的底层方法（主要用于更改连接，切换库，更改超时时间） |
>| IRepositoryMediator<> | RepositoryMediator<> | 仓储的中介者接口用于处理多个对象的操作 |
>| SlavePools | | 存储从库的连接信息，用于心跳检查操作 |
>| SlaveDbConnection |  | 从库的连接字符串的配置,在注入的时候将此信息写入到<b>SlavePools</b> |
>| UnitOfWorkOptions<> |  | 上下文工作单元的参数，存储在作用域操作中的仓储的一些字段信息 |
>| QueryableExtensions |  | linq查询扩展（分页，whereif，将IQueryable转换成Sql） |
>| RepositoryExtensions |  | 仓储层的依赖注入扩展 |
>## 使用说明
>> 1. 如需使用此层的数据访问，需要将<b>RepositoryExtensions</b>方法中的两个扩展方法，注入到Startup中，并配置参数，参数中可以选择是否开启读写分离，默认不开启
>>```c#
            //注入仓储依赖的服务   
            services.AddRepositoryServer()
            //注入ef配置信息
            .AddEFOption(options =>
            {
                options.ConfigureDbContext = context => context.UseMySql("Database=test;DataSource=127.0.0.1;Port=3306;UserId=;Password=;Charset=utf8;").AddInterceptors(new EFDbCommandInterceptor());
                options.ReadOnlyConnectionString = new string[] { "Database=;DataSource=;Port=3306;UserId=;Password=;Charset=utf8;" };
                //
                options.UseEntityFramework<MysqlDbContent, SlaveMysqlDbContent>(true, 100);
                options.IsOpenMasterSlave = true;
            });
>> 2. 当前仓储使用的数据库的读写分离,工作单元，CQRS,分库，仓储的生命周期为Scoped作用域，
>> 3. 所有的操作都需要使用<b>IUnitOfWork\<TDbContext></b>进行，其中<b>TDbContext为主库的上下文</b> ,具体操作请看单元测试中的使用