using System;

namespace Bitmex.Api.Models
{
	public sealed class CancelOrderRequest
	{
		public CancelOrderRequest(Guid orderId)
		{
			OrderId = orderId;
		}

		public Guid OrderId { get; }
	}
}