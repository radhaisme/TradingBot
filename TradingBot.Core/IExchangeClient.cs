using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Core.Entities;

namespace TradingBot.Core
{
	public interface IExchangeClient
	{
		//Task<OrderDetails> GetOrderInfoAsync(int orderId);
		//Task<CancelOrder> CancelOrderAsync(int orderId);
		//Task<CreateOrder> CreateOrderAsync(string pair, OrderType type, decimal price, decimal amount);
		//Task<Balance> GetInfoAsync();
		//Task<TradeInfo> GetTradesAsync(string pair, uint limit = 150);
		Task<IEnumerable<Pair>> GetPairs();
		Task<PairDetail> GetPairDetail(string pair);
		//Task<PairOrders> GetPairOrdersAsync(string pair, uint limit = 150);
		//Task<OrderDetails> GetActiveOrdersOfUserAsync(string pair);
	}
}