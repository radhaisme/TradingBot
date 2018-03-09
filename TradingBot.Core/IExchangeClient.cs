
namespace TradingBot.Core
{
	using Entities;
	using System.Threading.Tasks;

	public interface IExchangeClient
	{
		Task<OrderDetails> GetOrderInfoAsync(int orderId);
		Task<CancelOrder> CancelOrderAsync(int orderId);
		Task<CreateOrder> CreateOrderAsync(string pair, OrderType type, decimal price, decimal amount);
		Task<Balance> GetInfoAsync();
		Task<TradeInfo> GetTradesAsync(string pair, uint limit = 150);
		Task<PairsInfo> GetPairsAsync();
		Task<PairData> GetPairDataAsync(string pair);
		Task<PairOrders> GetPairOrdersAsync(string pair, uint limit = 150);
		Task<OrderDetails> GetActiveOrdersOfUserAsync(string pair);
	}
}