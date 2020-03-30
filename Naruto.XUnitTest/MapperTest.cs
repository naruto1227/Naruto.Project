using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Naruto.Mapper;
using Xunit;

using Naruto.Mapper.Attributes;
using AutoMapper;
using Naruto.Infrastructure.ExpressTree;
using System.Linq.Expressions;

namespace Naruto.XUnitTest
{
    [AutoInjectDto(SoureType = typeof(TestDto), TargetType = typeof(TestDto2), ReverseMap = true)]
    public class TestDto
    {
        public int MyProperty { get; set; }
        [AutoInjectIgnore]
        public int MyProperty2 { get; set; }

        public int mysdas;
    }

    public class TestDto2
    {
        public int MyProperty { get; set; }

        public int MyProperty2 { get; set; }
        public int mysdas;
    }
    [AutoInjectDto(SoureType = typeof(TestDto3), TargetType = typeof(TestDto4), ReverseMap = true)]
    public class TestDto3
    {
        public int MyProperty { get; set; }
        [AutoInjectIgnore]
        public int MyProperty2 { get; set; }
        [AutoInjectIgnore]
        public int mysdas;
    }

    public class TestDto4
    {
        public int MyProperty { get; set; }

        public int MyProperty2 { get; set; }
        public int mysdas;
    }
    public class MyProFile : Profile
    {
        public MyProFile()
        {
            CreateMap(typeof(TestDto), typeof(TestDto));
        }
    }

    public class TestDto7
    {
        public ICollection<TestDto8> li2 { get; set; }
        public List<TestDto8> li { get; set; }
        public int Id { get; set; }

        public string DuringTime { get; set; }

        public string Rule { get; set; }

        public string Contact { get; set; }

        public string Description { get; set; }
        public int Integral { get; set; }


        public int Integral2 { get; set; }

        public string test { get; set; }

        public long test2 { get; set; }

        public bool test3 { get; set; }

        public short test4 { get; set; }

        public byte test5 { get; set; }

        public uint test6 { get; set; }

        public ulong test7 { get; set; }
        public double test8 { get; set; }

        public decimal test9 { get; set; }

        public char test10 { get; set; }

        public object test11 { get; set; }

        public float test12 { get; set; }

        public byte[] test13 { get; set; }

        public DateTime test14 { get; set; } = default;

        public Guid guid { get; set; }

        public object o { get; set; }

        public TestDto8 settig2 { get; set; }
    }

    public class TestDto8
    {
        public ICollection<TestDto8> li2 { get; set; }
        public List<TestDto8> li { get; set; }
        public int Id { get; set; }

        public string DuringTime { get; set; }

        public string Rule { get; set; }

        public string Contact { get; set; }

        public string Description { get; set; }
        public int Integral { get; set; }


        public int Integral2 { get; set; }

        public string test { get; set; }

        public long test2 { get; set; }

        public bool test3 { get; set; }

        public short test4 { get; set; }

        public byte test5 { get; set; }

        public uint test6 { get; set; }

        public ulong test7 { get; set; }
        public double test8 { get; set; }

        public decimal test9 { get; set; }

        public char test10 { get; set; }

        public object test11 { get; set; }

        public float test12 { get; set; }

        public byte[] test13 { get; set; }

        public DateTime test14 { get; set; } = default;

        public Guid guid { get; set; }

        public object o { get; set; }

        public TestDto8 settig2 { get; set; }
    }
    public class MapperTest
    {
        IServiceCollection serviceDescriptors = new ServiceCollection();
        public MapperTest()
        {
            serviceDescriptors.AddRegisterMapper(typeof(MapperTest));

        }
        [Fact]
        public void ExpressionMapper()
        {
           // var ress = Expression.Convert(Expression.Constant(default), typeof(TestDto7));
            var res = ExpressionMapper<TestDto7, TestDto8>.To(new TestDto7()
            {
                Contact = "1",
                Description = ""
            });
        }
        [Fact]
        public void EntityMapper()
        {
            using (var service = serviceDescriptors.BuildServiceProvider().CreateScope())
            {
                var mapper = service.ServiceProvider.GetRequiredService<IEntityMapper>();

                var res = mapper.MapperTo<TestDto2>(new TestDto() { MyProperty = 1, mysdas = 1 });
                var res3 = mapper.MapperTo(new TestDto() { MyProperty = 2, MyProperty2 = 123123 }, res);
                var res2 = mapper.MapperTo<TestDto>(new TestDto() { MyProperty = 123123 });
                var res4 = mapper.MapperToList<TestDto>(new List<TestDto>() { new TestDto { MyProperty = 123123 } });

                var res5 = mapper.MapperTo<TestDto4>(new TestDto3() { MyProperty = 1, mysdas = 1 });
            }
        }
    }
}
