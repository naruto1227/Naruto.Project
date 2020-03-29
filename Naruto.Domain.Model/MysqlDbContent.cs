using System;
using System.Collections.Generic;
using System.Text;
using Naruto.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

//using Naruto.Domain.Model.Entities;

namespace Naruto.Domain.Model
{
    public class SlaveMysqlDbContent : DbContext
    {
        public SlaveMysqlDbContent(DbContextOptions<SlaveMysqlDbContent> options2)
          : base(options2)
        {

        }
        public DbSet<setting> setting { get; set; }
        public DbSet<test1> test1 { get; set; }
    }
    public class MysqlDbContent : DbContext
    {
        public MysqlDbContent(DbContextOptions<MysqlDbContent> options)
           : base(options)
        {

        }
        public MysqlDbContent Clone()
        {
            return this.MemberwiseClone() as MysqlDbContent;
        }
        //public DbSet<OrderNo> OrderNo { get; set; }
        public DbSet<setting> setting { get; set; }
        public DbSet<test1> test1 { get; set; }

    }

    public class TestDbContent : DbContext
    {
        public TestDbContent(DbContextOptions<TestDbContent> options)
           : base(options)
        {

        }
        public TestDbContent Clone()
        {
            return this.MemberwiseClone() as TestDbContent;
        }
        //public DbSet<OrderNo> OrderNo { get; set; }
        public DbSet<setting> setting { get; set; }
        public DbSet<test1> test1 { get; set; }

    }

    public class SlaveTestDbContent : DbContext
    {
        public SlaveTestDbContent(DbContextOptions<SlaveTestDbContent> options)
           : base(options)
        {

        }
        public SlaveTestDbContent Clone()
        {
            return this.MemberwiseClone() as SlaveTestDbContent;
        }
        //public DbSet<OrderNo> OrderNo { get; set; }
        public DbSet<setting> setting { get; set; }
        public DbSet<test1> test1 { get; set; }

    }
}
