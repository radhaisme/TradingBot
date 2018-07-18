using System.Collections.Generic;

namespace TradingBot.Core.Entities
{
	public class CreateOrderDto
	{
		public CreateOrderDto()
		{
			Funds = new Dictionary<string, decimal>();
		}

		public decimal Received { get; set; }
		public decimal Remains { get; set; }
		public int OrderId { get; set; }
		public IDictionary<string, decimal> Funds { get; }
	}
}