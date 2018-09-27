
namespace Yobit.Api.Models
{
	public sealed class CreateOrderRequest
	{
		public CreateOrderRequest(string pair, TradeType tradeType, decimal rate, decimal amount)
		{
			Pair = pair;
			TradeType = tradeType;
			Rate = rate;
			Amount = amount;
		}

		public string Pair { get; }
		public TradeType TradeType { get; }
		public OrderType OrderType { get; set; }
		public decimal Rate { get; }
		public decimal Amount { get; }
	}
}