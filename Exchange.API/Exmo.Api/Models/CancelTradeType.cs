using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Exmo.Api.Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum CancelTradeType : byte
	{
		Trade,
		TradePair,
		All
	}
}