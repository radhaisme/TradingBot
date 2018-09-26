using System.Collections.Generic;

namespace TradingBot.Core.Entities
{
	public class OrderResponse
	{
		public OrderResponse()
		{
			Orders = new List<Trade>().AsReadOnly();
		}

		public IReadOnlyCollection<Trade> Orders { get; set; }
	}
}