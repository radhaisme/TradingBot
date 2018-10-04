using System;
using Newtonsoft.Json;

namespace TradingBot.Api.Helpers
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

		public static T FromJson<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}

		public static object FromJson(string json, Type type)
		{
			return JsonConvert.DeserializeObject(json, type);
		}

		public static T FromJson<T>(string json, JsonSerializerSettings settings)
		{
			return JsonConvert.DeserializeObject<T>(json, settings);
		}
	}
}