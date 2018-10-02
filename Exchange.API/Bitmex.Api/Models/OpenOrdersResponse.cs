using System.Collections.Generic;

namespace Bitmex.Api.Models
{
	public sealed class OpenOrdersResponse
	{
		public OpenOrdersResponse(List<OrderResult> orders)
		{
			Orders = orders.AsReadOnly();
		}

		public IReadOnlyCollection<OrderResult> Orders { get; }
	}
}