using Naruto.BaseMongo.Model;
using Naruto.MongoDB.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naruto.Test.TestClass
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
