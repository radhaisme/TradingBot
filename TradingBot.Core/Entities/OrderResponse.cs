using System.Collections.Generic;

namespace TradingBot.Core.Entities
{
	public class OrderResponse
	{
		public OrderResponse()
		{
			Orders = new List<Order>().AsReadOnly();
		}

		public IReadOnlyCollection<Order> Orders { get; set; }
	}
}