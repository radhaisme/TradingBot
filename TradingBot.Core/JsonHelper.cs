
namespace TradingBot.Core
{
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

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
			return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings());
		}

		public static T FromJson<T>(string json, JsonSerializerSettings settings)
		{
			return JsonConvert.DeserializeObject<T>(json, settings);
		}
	}
}