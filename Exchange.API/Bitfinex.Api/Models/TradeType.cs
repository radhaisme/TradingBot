using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bitfinex.Api.Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TradeType : byte
	{
		Buy,
		Sell
	}
}