using System.Threading.Tasks;
using TradingBot.Core.Models;

namespace TradingBot.Core.Proxies
{
	public interface IProxy
	{
		//Task<OrderDetails> GetOrderInfoAsync(int orderId);
		//Task<CancelOrderRequest> CancelOrderAsync(int orderId);
		//Task<CreateOrderResponse> CreateOrderAsync(string pair, TradeType type, decimal price, decimal amount);
		//Task<Balance> GetInfoAsync();
		//Task<OrderResponse> GetTradesAsync(string pair, uint limit = 150);
		Task<TradePairsResponse> GetTradePairsAsync();
		//Task<PairDetailResponse> GetPairDetailAsync(PairDetailRequest request);
		//Task<DepthResponse> GetOrderBookAsync(DepthRequest request);
		//Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request);
		//Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request);
		//Task<IReadOnlyCollection<PairDetail>> GetPairsDetails(params string[] pairs);
		//Task<DepthResponse> GetPairOrdersAsync(string pair, uint limit = 150);
		//Task<OrderDetails> GetActiveOrdersOfUserAsync(string pair);
	}
}