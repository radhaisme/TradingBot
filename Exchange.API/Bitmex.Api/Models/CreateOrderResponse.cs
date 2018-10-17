using System;

namespace Bitmex.Api.Models
{
	public sealed class CreateOrderResponse
	{
		public CreateOrderResponse(Guid orderId)
		{
			OrderId = orderId;
		}

		public Guid OrderId { get; }
	}
}