using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TradingBot.Core.Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TradeType : byte
	{
		Buy,
		Sell
	}
}