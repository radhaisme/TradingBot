
namespace Cryptopia.Api.Models
{
	public sealed class CreateOrderRequest
	{
		public CreateOrderRequest(string market, TradeType type, decimal rate, decimal amount)
		{
			Market = market;
			Type = type;
			Rate = rate;
			Amount = amount;
		}

		public CreateOrderRequest(int tradepairId, TradeType type, decimal rate, decimal amount) : this(null, type, rate, amount)
		{
			TradePairId = tradepairId;
		}

		public int? TradePairId { get; }
		public string Market { get; }
		public TradeType Type { get; }
		public decimal Rate { get; }
		public decimal Amount { get; }
	}
}