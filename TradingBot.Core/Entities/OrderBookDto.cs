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

		public List<OrderDto> Asks { get; }
		public List<OrderDto> Bids { get; }
	}
}