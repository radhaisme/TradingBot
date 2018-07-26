using System;
using System.Collections.Generic;
using System.Linq;
using TradingBot.Core.Entities;

namespace TradingBot.Core
{
	public static class Helper
	{
		public static DepthDto BuildOrderBook(IEnumerable<dynamic> a, IEnumerable<dynamic> b, Func<dynamic, BookOrderDto> func)
		{
			if (func == null)
			{
				throw new ArgumentNullException(nameof(func));
			}

			var model = new DepthDto();
			IEnumerable<BookOrderDto> asks = a.Select(func).Where(x => x.Price > 0);
			IEnumerable<BookOrderDto> bids = b.Select(func).Where(x => x.Price > 0);

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