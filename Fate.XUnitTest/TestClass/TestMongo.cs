using Fate.Infrastructure.BaseMongo.Model;
using Fate.Infrastructure.MongoDB.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fate.Test.TestClass
{
    public class TestMongo: MongoContext
    {

    }

    public class TestDTO : IMongoEntity
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
