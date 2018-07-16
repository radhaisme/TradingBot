using System;
using System.Threading.Tasks;
using Kucoin.Api;
using Okex.Api;
using TradingBot.CurrencyProvider;
using TradingBot.Scanner;
using Yobit.Api;

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

			var client = new OkexClient();
			var r = await client.GetOrderBookAsync("ltc_btc");

			Console.ReadLine();
		}
	}
}