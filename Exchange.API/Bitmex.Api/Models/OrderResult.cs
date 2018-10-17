using System;

namespace Bitmex.Api.Models
{
	public sealed class OrderResult
	{
		public OrderResult(string orderId)
		{
			OrderId = Guid.Parse(orderId);
		}

		public Guid OrderId { get; }
		public string Pair { get; set; }
		public TradeType TradeType { get; set; }
		public decimal Rate { get; set; }
		public decimal Amount { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}