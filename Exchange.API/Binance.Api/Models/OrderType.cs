using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Binance.Api.Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum OrderType : byte
	{
		Limit,
		Market
	}
}