
namespace Yobit.Api
{
	using Newtonsoft.Json;
	using System;
	using System.Reflection;

	public class YobitResponseConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var model = new YobitResponse();
			reader.Read();
			bool? value = reader.ReadAsBoolean();
			model.Success = value.GetValueOrDefault();
			reader.Read();
			reader.Read();

			if (model.Success)
			{
				model.Content = serializer.Deserialize(reader).ToString();
			}
			else
			{
				model.Error = serializer.Deserialize(reader).ToString();
			}
			
			return model;
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(YobitResponse) == objectType;
		}
	}
}