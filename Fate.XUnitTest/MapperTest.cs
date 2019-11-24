using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Fate.Infrastructure.Mapper;
using Xunit;

using Fate.Infrastructure.Mapper.Attributes;
using AutoMapper;

namespace Fate.XUnitTest
{
    [AutoInjectDto(SoureType = typeof(TestDto), TargetType = typeof(TestDto2), ReverseMap = true)]
    public class TestDto
    {
        public int MyProperty { get; set; }
        [AutoInjectIgnore]
        public int MyProperty2 { get; set; }
    }

    public class TestDto2
    {
        public int MyProperty { get; set; }

        public int MyProperty2 { get; set; }
    }

    public class MyProFile : Profile
    {
        public MyProFile()
        {
            CreateMap(typeof(TestDto), typeof(TestDto));
        }
    }
    public class MapperTest
    {
        IServiceCollection serviceDescriptors = new ServiceCollection();
        public MapperTest()
        {
            serviceDescriptors.AddRegisterMapper(typeof(MapperTest));

        }

        [Fact]
        public void EntityMapper()
        {
            using (var service = serviceDescriptors.BuildServiceProvider().CreateScope())
            {
                var mapper = service.ServiceProvider.GetRequiredService<IEntityMapper>();

                var res = mapper.MapperTo<TestDto2>(new TestDto() { MyProperty = 1 });
                var res3 = mapper.MapperTo(new TestDto() { MyProperty = 2, MyProperty2 = 123123 }, res);
                var res2 = mapper.MapperTo<TestDto>(new TestDto() { MyProperty = 123123 });
                var res4 = mapper.MapperToList<TestDto>(new List<TestDto>() { new TestDto { MyProperty = 123123 } });
            }
        }
    }
}
