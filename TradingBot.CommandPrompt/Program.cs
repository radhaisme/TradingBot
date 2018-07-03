using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingBot.Core.Entities;
using Pair = TradingBot.Data.Entities.Pair;

namespace TradingBot.CommandPrompt
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			var c1 = new Currency(1, "BTC", "Bitcoin");
			var c2 = new Currency(2, "ETH", "Ether");
			var a = new Pair(c2, c1);
			var b = new Pair(c2, c1);

			var pairs = new List<Pair> { a, b };
			var r = pairs.Intersect(new List<Pair> { a, b }).ToList();

			Console.ReadLine();
		}
	}
}