## 接口对象说明
>| 类 | 注释 |
>| :-----:| :----: |
>| IVirtualFileRender | 虚拟资源渲染的接口层 |
>| DefaultVirtualFileRender | 虚拟资源渲染的默认实现 |
>| IVirtualFileResource | 虚拟资源文件的信息操作接口 |
>| DefaultVirtualFileResource | 虚拟资源文件的信息操作默认实现 |
>| IVirtualFileRouteCollections | 存在虚拟资源请求路由的接口层 |
>| DefaultVirtualFileRouteCollections | 存放静态资源的路由集合的默认实现 |
>| IVirtualFileAuthorizationFilters | 虚拟文件系统的授权接口过滤接口 |
>| VirtualFileContext | 虚拟文件系统的上下文 |
>| VirtualFileOptions | 虚拟文件系统的参数配置 |
>| ResoureInfo | 虚拟文件系统的资源信息静态初始化对象(<b>首先需要往此类添加资源的信息</b>) |
>| VirtualFileStartupFilter | 注入静态中间件 |
>| VirtualFileMiddleware | 虚拟资源中间件 |

## 使用说明
>     1.使用前应该先将需要访问的虚拟资源信息存放进ResoureInfo集合中
>     2.然后再注入AddVirtualFileServices方法