using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace TradingBot.Core
{
	public interface IApiClient
	{
		ExchangeType Type { get; }
		//Task<OrderDetails> GetOrderInfoAsync(int orderId);
		//Task<CancelOrderDto> CancelOrderAsync(int orderId);
		//Task<CreateOrderDto> CreateOrderAsync(string pair, Side type, decimal price, decimal amount);
		//Task<Balance> GetInfoAsync();
		//Task<TradeInfo> GetTradesAsync(string pair, uint limit = 150);
		Task<IReadOnlyCollection<PairDto>> GetPairsAsync();
		Task<PairDetailDto> GetPairDetailAsync(string pair);
		Task<DepthDto> GetOrderBookAsync(string pair, uint limit = 100);
		Task<CreateOrderDto> CreateOrderAsync(OrderDto input);
		Task<CancelOrderDto> CancelOrderAsync(CancelOrderDto input);
		//Task<IReadOnlyCollection<PairDetail>> GetPairsDetails(params string[] pairs);
		//Task<DepthDto> GetPairOrdersAsync(string pair, uint limit = 150);
		//Task<OrderDetails> GetActiveOrdersOfUserAsync(string pair);
	}
}