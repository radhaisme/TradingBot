using System;
using System.Collections.Generic;
using System.Linq;
using TradingBot.Core.Entities;

namespace TradingBot.Core
{
	public static class Helper
	{
		public static OrderBookDto BuildOrderBook(IEnumerable<dynamic> a, IEnumerable<dynamic> b, Func<dynamic, OrderDto> func)
		{
			if (func == null)
			{
				throw new ArgumentNullException(nameof(func));
			}

			var model = new OrderBookDto();
			IEnumerable<OrderDto> asks = a.Select(func).Where(x => x.Price > 0);
			IEnumerable<OrderDto> bids = b.Select(func).Where(x => x.Price > 0);

			if (!asks.Any() || !bids.Any())
			{
				return model;
			}

			if (asks.Count() < bids.Count())
			{
				bids = bids.Take(asks.Count());
			}
			else
			{
				asks = asks.Take(bids.Count());
			}

			model.Asks.AddRange(asks);
			model.Bids.AddRange(bids);

			return model;
		}
	}
}