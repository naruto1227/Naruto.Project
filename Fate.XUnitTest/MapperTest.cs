using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Fate.Common.Mapper;
using Xunit;

using Fate.Common.Mapper.Attributes;
using AutoMapper;

namespace Fate.XUnitTest
{
    [AutoInjectDto(SoureType = typeof(TestDto), TargetType = typeof(TestDto2), ReverseMap = true)]
    public class TestDto
    {
        public int MyProperty { get; set; }
    }

    public class TestDto2
    {
        public int MyProperty { get; set; }
    }

    public class MyProFile : Profile
    {
        public MyProFile()
        {
            CreateMap(typeof(TestDto), typeof(TestDto2));
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
                mapper.MapperTo(new TestDto() { MyProperty = 2 }, res);

            }
        }
    }
}
