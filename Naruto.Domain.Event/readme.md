>## 使用说明
>> 1. 事件的handler 处理类 需放在 <b>EventHandlers</b>文件夹中，并且需要继承<b>IEventHandler</b>
>> 2. 事件源信息 需放置于 Events文件夹中 并且需要继承<b>EventData</b> 抽象，需包含一个无参，一个有参的构造函数，已实现工厂模式的赋值
>> 3. <b>Infrastructure.Redis</b>文件夹中是已redis做为底板的 事件总线
>> 4. <b>EventBus</b> 是以应用程序内存作为存储的时间总线