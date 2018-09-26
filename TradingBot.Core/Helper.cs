using System;
using System.Collections.Generic;
using System.Linq;

namespace TradingBot.Core
{
	public static class Helper
	{
		public static U BuildOrderBook<U, T>(IEnumerable<dynamic> asks, IEnumerable<dynamic> bids, uint limit, Func<dynamic, T> func) where U : new()
		{
			if (func == null)
			{
				throw new ArgumentNullException(nameof(func));
			}
			
			//var model = new U();
			var a = asks.Select(func).Take((int)limit); //.Where(x => x.Price > 0);
			var b = bids.Select(func).Take((int) limit); //.Where(x => x.Price > 0);

			//if (!asks.Any() || !bids.Any())
			//{
			//	return model;
			//}

			//if (asks.Count() < bids.Count())
			//{
			//	bids = bids.Take(asks.Count());
			//}
			//else
			//{
			//	asks = asks.Take(bids.Count());
			//}

			//model.Asks.AddRange(asks);
			//model.Bids.AddRange(bids);

			//return model;

			return default(U);
		}
	}
}