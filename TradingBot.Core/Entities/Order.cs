
namespace TradingBot.Core.Entities
{
	using System;

	public class Order
	{
		public string Pair { get; set; }
		public OrderType Type { get; set; }
		public decimal Amount { get; set; }
		public decimal Rate { get; set; }
		public DateTimeOffset TimestampCreated { get; set; }
	}
}