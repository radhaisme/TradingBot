using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TradingBot.Core;
using TradingBot.Core.Enums;
using TradingBot.CurrencyProvider;
using TradingBot.Scanner;

namespace TradingBot.CommandPrompt
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			var provider = new CoinMarketCapClient();
			//var factory = new ExchangeFactory(provider);
			//var scanner = new ArbitrageScanner(factory);
			//scanner.Start();
			
			Console.ReadLine();
		}
	}
}