using System;

namespace Exmo.Api.Models
{
	public class OrderResult
	{
		public OrderResult(long orderId)
		{
			OrderId = orderId;
		}

		public long OrderId { get; }
		public string Pair { get; set; }
		public TradeType TradeType { get; set; }
		public OrderType OrderType { get; set; }
		public decimal Rate { get; set; }
		public decimal Amount { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}