using System.Collections.Generic;

namespace TradingBot.Core.Models
{
	public class OrderDetails
	{
		public OrderDetails()
		{
			Orders = new Dictionary<int, OrderInfo>();
		}

		public IDictionary<int, OrderInfo> Orders { get; }
	}
}