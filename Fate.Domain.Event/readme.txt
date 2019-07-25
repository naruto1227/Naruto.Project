1.事件的handler 处理类 需放在 EventHandlers文件夹中，并且需要继承IEventHandler
2.事件源信息 需放置于 Events文件夹中 并且需要继承EventData 抽象，需包含一个无参，一个有参的构造函数，已实现工厂模式的赋值
3.Infrastructure.Redis文件夹中是已redis做为地板的 事件总线
4.EventBus 是以应用程序内存作为存储的时间总线