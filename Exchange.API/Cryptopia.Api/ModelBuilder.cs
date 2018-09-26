using Cryptopia.Api.Models;
using TradingBot.Core.Entities;

namespace Cryptopia.Api
{
	internal class ModelBuilder
	{
		public Order CreateOrder(CreateOrderRequest request)
		{
			//TODO: Place an order validation to here

			return new Order
			{
				Market = request.Pair,
				Type = request.TradeType,
				Rate = request.Rate,
				Amount = request.Amount
			};
		}
	}
}