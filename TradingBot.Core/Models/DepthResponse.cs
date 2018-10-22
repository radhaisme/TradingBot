using System.Collections.Generic;

namespace TradingBot.Core.Models
{
	public class DepthResponse
	{
		public DepthResponse()
		{
			Asks = new List<BookOrderDto>();
			Bids = new List<BookOrderDto>();
		}

		public List<BookOrderDto> Asks { get; }
		public List<BookOrderDto> Bids { get; }
	}
}