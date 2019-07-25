1.数据库实体需要继承 IEntity
2.其它的需要实现依赖注入的实体 只需继承IModelDependency 即可 
3 .依赖注入的生命周期为瞬时