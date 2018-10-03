using System;

namespace Yobit.Api.Models
{
	public class OrderResult
	{
		public OrderResult(long orderId)
		{
			OrderId = orderId;
		}

		public string Pair { get; set; }
		public long OrderId { get; }
		public TradeType TradeType { get; set; }
		public OrderType OrderType { get; set; }
		public decimal Rate { get; set; }
		public decimal Amount { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}