using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Configuration
{
    public static class JsonUtil
    {
        public static string ToJson(this object soure)
        {
            if (soure == null)
                return default;

            return JsonConvert.SerializeObject(soure);
        }

        public static T ToObject<T>(this string soure) where T : class
        {
            if (soure == null)
                return default;
            return JsonConvert.DeserializeObject<T>(soure);
        }
    }
}
