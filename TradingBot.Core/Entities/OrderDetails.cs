
namespace TradingBot.Core.Entities
{
	using System.Collections.Generic;

	public class OrderDetails
	{
		public OrderDetails()
		{
			Orders = new Dictionary<int, OrderInfo>();
		}

		public IDictionary<int, OrderInfo> Orders { get; }
	}
}