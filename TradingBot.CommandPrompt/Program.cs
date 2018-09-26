using Binance.Api;
using System;
using System.Threading.Tasks;
using TradingBot.Core.Entities;

namespace TradingBot.CommandPrompt
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			var client = new BinanceClient();
			var r = await client.CancelOrderAsync(new CancelOrderRequest { Pair = "ETHBTC", OrderId = 1 });

			//var r = await client.CreateOrderAsync(new CreateOrderRequest { Pair = "ETHBTC", Rate = 0.1m, Amount = 1, TradeType = TradeType.Buy, Type = OrderType.Limit });



			Console.ReadLine();
		}
	}
}