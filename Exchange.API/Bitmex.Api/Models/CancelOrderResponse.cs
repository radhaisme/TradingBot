using System;

namespace Bitmex.Api.Models
{
	public sealed class CancelOrderResponse
	{
		public CancelOrderResponse(Guid orderId)
		{
			OrderId = orderId;
		}

		public Guid OrderId { get; }
	}
}