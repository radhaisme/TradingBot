
namespace TradingBot.Core.Entities
{
	public class CancelOrderDto
	{
		public string Pair { get; set; }
		public int TradePairId { get; set; }
		public OrderType Type { get; set; }
		public long OrderId { get; set; }
	}
}