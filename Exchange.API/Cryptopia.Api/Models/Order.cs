using TradingBot.Core.Entities;

namespace Cryptopia.Api.Models
{
	internal class Order
	{
		public Order(string market, TradeType type, decimal rate, decimal amount)
		{
			Market = market;
			Type = type;
			Rate = rate;
			Amount = amount;
		}

		public Order(int tradepairId, TradeType type, decimal rate, decimal amount) : this(null, type, rate, amount)
		{
			TradePairId = tradepairId;
		}

		public int? TradePairId { get; }
		public string Market { get; set; }
		public TradeType Type { get; set; }
		public decimal Rate { get; set; }
		public decimal Amount { get; set; }
	}
}