using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Core.Entities;

namespace TradingBot.Core
{
	public interface IExchangeClient
	{
		//Task<OrderDetails> GetOrderInfoAsync(int orderId);
		//Task<CancelOrderDto> CancelOrderAsync(int orderId);
		//Task<CreateOrderDto> CreateOrderAsync(string pair, OrderType type, decimal price, decimal amount);
		//Task<Balance> GetInfoAsync();
		//Task<TradeInfo> GetTradesAsync(string pair, uint limit = 150);
		Task<IReadOnlyCollection<PairDto>> GetPairsAsync();
		Task<PairDetailDto> GetPairDetailAsync(string pair);
		Task<OrderBookDto> GetOrderBookAsync(string pair, uint limit = 100);
		//Task<IReadOnlyCollection<PairDetail>> GetPairsDetails(params string[] pairs);
		//Task<OrderBookDto> GetPairOrdersAsync(string pair, uint limit = 150);
		//Task<OrderDetails> GetActiveOrdersOfUserAsync(string pair);
	}
}