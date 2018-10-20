using Bitmex.Api;
using System;
using System.Threading.Tasks;
using Bitmex.Api.Models;

namespace TradingBot.CommandPrompt
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			var client = new BitmexClient();
			var r = await client.GetOrderBookAsync(new DepthRequest { Pair = "XBTUSD", Limit = 25 });

			//var r2 = await client.GetOpenOrdersAsync(new OpenOrdersRequest("XBTUSD"));

			//foreach (var pair in r.Pairs)
			//{
			//	Console.WriteLine($"{pair.Symbol}/{pair.QuoteAsset+pair.BaseAsset}");
			//}

			Console.ReadLine();
		}
	}
}