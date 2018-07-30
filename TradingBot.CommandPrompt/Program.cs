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
			var r = await client.CancelOrderAsync(new CancelOrderDto { Pair = "ETHBTC", OrderId = 1 });

			//var r = await client.CreateOrderAsync(new OrderDto { Pair = "ETHBTC", Price = 0.1m, Amount = 1, Side = Side.Buy, Type = OrderType.Limit });



			Console.ReadLine();
		}
	}
}