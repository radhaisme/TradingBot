using Newtonsoft.Json;

namespace Bitfinex.Api.Models
{
	public sealed class CreateOrderRequest : OrderRequest
	{
		public CreateOrderRequest(string pair, TradeType tradeType, decimal rate, decimal amount)
		{
			Pair = pair;
			TradeType = tradeType;
			Rate = rate;
			Amount = amount;
		}

		[JsonProperty(PropertyName = "symbol")]
		public string Pair { get; }

		[JsonProperty(PropertyName = "side")]
		public TradeType TradeType { get; }
		
		[JsonProperty(PropertyName = "type")]
		public OrderType OrderType { get; set; }

		[JsonProperty(PropertyName = "price")]
		public decimal Rate { get; }

		[JsonProperty(PropertyName = "amount")]
		public decimal Amount { get; }

		[JsonProperty(PropertyName = "ocoorder")]
		public bool Ocoorder => false;
	}
}