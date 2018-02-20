
namespace TradingBot.Core.Entities
{
	using System.Collections.Generic;

	public class CancelOrder
	{
		public CancelOrder()
		{
			Funds = new Dictionary<string, decimal>();
		}

		public int OrderId { get; set; }
		public IDictionary<string, decimal> Funds { get; set; }
	}
}