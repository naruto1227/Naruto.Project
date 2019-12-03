## 接口对象说明
>| 类 | 注释 |
>| :-----:| :----: |
>| MongoDependencyExtension | 依赖注入扩展 |

## 注意细节
>1.  当从mongodb中取数据的时候，如果实体类中不存在mongo中的字段，不然序列化的时候报错，mongodb中的字段必须都包含在实体类中。
>2.   mongodb读取的时候区分大小写。
>3.   实体中必须包含一个_id 为ObjectId的字段 (这个地方只需要所有的实体继承<b>IMongoEntity</b>对象即可)
>4. ObjectId 不可更改