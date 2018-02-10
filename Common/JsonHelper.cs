using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Common
{
    public static class JsonHelper
    {
        public static string ToJson(object @object)
        {
            return JsonConvert.SerializeObject(@object);
        }

        public static string ToJson(object @object, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(@object, settings);
        }

        public static T ToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T ToObject<T>(string json, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static object ToObject(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }
    }

}
