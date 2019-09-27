># 实体映射需要引用Fate.Common.Mapper层
>> ## 需要在StartUp中注入AddRegisterMapper ,包含一个参数<b>typesParams</b>（传递当前需要实现映射的层的一个对象的Type即可,每个层只需要传递一个type）
># 1.自动化批量注入映射关系
>* ## 特性讲解
>>   1. <b>AutoInjectDtoAttribute</b> 标记当前Dto 为自动注入映射关系的对象  
>><sub>1.1. 字段讲解</sub>
>>>    - <b>TargetType</b> 字段标识需要映射到的目标框架的
>>>    - <b>SoureType</b> 字段标识需要映射的源对象的类型
>>>   - <b>ReverseMap</b> 字段标识对象之间是否可以双向建立映射关系
>>   2. <b>字段的特性</b>
>>>    - <b>AutoInjectIgnoreAttribute</b> 特性标识当前字段是否在映射的时候是否忽略(此功能目前仅支持单向的<b>ReverseMap</b>字段的设置对其无效)
># 2.通过配置(<b>Profile</b>）来注入映射关系
>> 例
>> ```
    //标记当前类继承Profile并在构造函数中创建映射关系
    public class MyProfile : Profile
    {
        public MyProfile()
        {
            var soure = typeof(ClientScopesDTO);
            var des = typeof(ApiClaims);
            //创建映射关系
            CreateMap(soure, des);
            //CreateMap(soure, typeof(ClientScopesDTO2)).ReverseMap();
        }

        public string str { get; set; }
    }
>> ```