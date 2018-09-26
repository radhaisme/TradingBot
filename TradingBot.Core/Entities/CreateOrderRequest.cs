
namespace TradingBot.Core.Entities
{
	public class CreateOrderRequest
	{
		public int AccountId { get; set; }
		public string Pair { get; set; }
		public OrderType Type { get; set; }
		public decimal Rate { get; set; }
		public decimal Amount { get; set; }
		public TradeType TradeType { get; set; }
	}
}