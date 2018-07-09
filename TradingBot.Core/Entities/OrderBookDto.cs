using System.Collections.Generic;

namespace TradingBot.Core.Entities
{
	public class OrderBookDto
	{
		public OrderBookDto()
		{
			Asks = new List<OrderDto>();
			Bids = new List<OrderDto>();
		}

		public ICollection<OrderDto> Asks { get; set; }
		public ICollection<OrderDto> Bids { get; set; }
	}
}