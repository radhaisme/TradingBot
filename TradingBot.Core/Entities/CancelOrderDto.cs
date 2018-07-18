using System.Collections.Generic;

namespace TradingBot.Core.Entities
{
	public class CancelOrderDto
	{
		public CancelOrderDto()
		{
			Funds = new Dictionary<string, decimal>();
		}

		public int OrderId { get; set; }
		public IDictionary<string, decimal> Funds { get; }
	}
}