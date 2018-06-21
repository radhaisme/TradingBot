using System;
using System.Threading.Tasks;
using Bitfinex.Api;

namespace TradingBot.CommandPrompt
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			var client = new BitfinexClient();
			var r = await client.GetPairs();

			Console.ReadLine();
		}
	}
}