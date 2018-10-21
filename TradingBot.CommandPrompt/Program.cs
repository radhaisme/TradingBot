using Bitmex.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bitmex.Api.Models;

namespace TradingBot.CommandPrompt
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			var client = new BitmexClient();
			var r = await client.GetMarketAsync(new MarketRequest("XBTUSD"));

			//var minRate = 200.1m;
			//var maxRate = 200.5m;
			//var amount = 0.0987m;
			//var ordersCount = 5;

			//var spread = maxRate - minRate;
			//var ratePerOrder = spread / ordersCount;
			//var amountPerOrder = amount / 5;

			//var orders = new List<CreateOrderRequest>(ordersCount);

			//for (var i = 0; i < ordersCount; i++)
			//{
			//	orders.Add(new CreateOrderRequest("XBTUSD", TradeType.Buy, ratePerOrder, amountPerOrder));
			//}

			//var rateResult = 0m;
			//var amountResult = 0m;

			//foreach (var order in orders)
			//{
			//	rateResult += order.Rate;
			//	amountResult += order.Amount;
			//}

			//var r = amount == amountResult;

			Console.ReadLine();
		}
	}

	public class ScaledOrder
	{
		private readonly List<CreateOrderRequest> _orders;

		public ScaledOrder(string pair, decimal minRate, decimal maxRate, decimal amount, int count, TradeType tradeType)
		{
			_orders = new List<CreateOrderRequest>(count);

			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			if (minRate > maxRate)
			{
				throw new ArgumentOutOfRangeException(nameof(minRate));
			}

			if (amount <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(amount));
			}

			count = count <= 0 ? 1 : count;
			var spread = maxRate - minRate;
			var ratePerOrder = spread / count;
			var amountPerOrder = amount / count;

			for (var i = 0; i < count; i++)
			{
				_orders.Add(new CreateOrderRequest("XBTUSD", tradeType, ratePerOrder, amountPerOrder));
			}
		}

		public IReadOnlyCollection<CreateOrderRequest> Orders => _orders.AsReadOnly();
	}
}