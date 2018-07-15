using System;
using System.Threading.Tasks;
using Kucoin.Api;
using TradingBot.CurrencyProvider;
using TradingBot.Scanner;

namespace TradingBot.CommandPrompt
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			//var provider = new CoinMarketCapClient();
			//var factory = new ExchangeFactory(provider);
			//var scanner = new ArbitrageScanner(factory);
			//scanner.Start();

			var client = new KucoinClient();
			//var r = await client.GetPairsAsync();
			var r = await client.GetOrderBookAsync("ETH-BTCH");

			Console.ReadLine();
		}
	}
}