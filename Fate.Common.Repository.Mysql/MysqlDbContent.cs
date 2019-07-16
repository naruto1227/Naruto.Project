using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

using Fate.Domain.Model.Entities;

namespace Fate.Common.Repository.Mysql
{
    public class MysqlDbContent : DbContext
    {
        public MysqlDbContent(DbContextOptions<MysqlDbContent> options)
           : base(options)
        {

        }
        public DbSet<OrderNo> OrderNo { get; set; }
        public DbSet<setting> setting { get; set; }
        public DbSet<test1> test1 { get; set; }

    }
}
