namespace TradingBot.Core.Models
{
	public class CancelOrderRequest
	{
		public string Pair { get; set; }
		public int TradePairId { get; set; }
		public OrderType Type { get; set; }
		public long OrderId { get; set; }
	}
}