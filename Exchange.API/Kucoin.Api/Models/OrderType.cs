using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kucoin.Api.Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum OrderType : byte
	{
		Limit,
		Market
	}
}