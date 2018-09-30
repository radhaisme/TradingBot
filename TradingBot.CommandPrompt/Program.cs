using Bitmex.Api;
using System;
using System.Threading.Tasks;

namespace TradingBot.CommandPrompt
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			var client = new BitmexClient();
			var r = await client.GetTradePairsAsync();

			foreach (var pair in r.Pairs)
			{
				Console.WriteLine($"{pair.Symbol}/{pair.QuoteAsset+pair.BaseAsset}");
			}

			Console.ReadLine();
		}
	}
}