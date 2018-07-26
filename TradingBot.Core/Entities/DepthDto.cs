using System.Collections.Generic;

namespace TradingBot.Core.Entities
{
	public class DepthDto
	{
		public DepthDto()
		{
			Asks = new List<BookOrderDto>();
			Bids = new List<BookOrderDto>();
		}

		public List<BookOrderDto> Asks { get; }
		public List<BookOrderDto> Bids { get; }
	}
}