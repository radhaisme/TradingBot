using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binance.Api;
using Bitfinex.Api;
using TradingBot.Core.Entities;

namespace TradingBot.CommandPrompt
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			var binance = new BinanceClient();
			var binancePairs = await binance.GetPairs();

			var bitfinex = new BitfinexClient();
			var bitfinexPairs = await bitfinex.GetPairs();

			var pairs = binancePairs.Intersect(bitfinexPairs).ToList();


			//var pairs = new List<Pair>();

			//foreach (Pair first in binancePairs)
			//{
			//	foreach (Pair second in bitfinexPairs)
			//	{
			//		if (first.Equals(second))
			//		{
			//			pairs.Add(first);
			//		}
			//	}
			//}

			Console.ReadLine();
		}
	}

	public class PairComparer : IEqualityComparer<Pair>
	{
		public bool Equals(Pair x, Pair y)
		{
			return x.Symbol.Equals(y.Symbol, StringComparison.InvariantCultureIgnoreCase);
		}

		public int GetHashCode(Pair obj)
		{
			return obj.GetHashCode();
		}
	}
}