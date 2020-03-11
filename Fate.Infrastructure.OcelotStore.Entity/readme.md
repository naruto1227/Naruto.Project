>## 使用说明
>> 1. ocelot的配置信息 的持久化存储在数据库中 ，然后数据都存储在<b>OcelotConfiguration</b>数据表中，对其他表做出的更改应在最后的时候，将所有表的数据汇聚到<b>OcelotConfiguration</b>表