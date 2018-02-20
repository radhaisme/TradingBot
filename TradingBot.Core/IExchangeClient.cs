
namespace TradingBot.Core
{
	using Entities;
	using System.Threading.Tasks;

	public interface IExchangeClient
	{
		Task<OrderDetails> GetOrderInfoAsync(int orderId);
		OrderDetails GetOrderInfo(int orderId);
		Task<CancelOrder> CancelOrderAsync(int orderId);
		CancelOrder CancelOrder(int orderId);
		Task<CreateOrder> CreateOrderAsync(string pair, OrderType type, decimal price, decimal amount);
		CreateOrder CreateOrder(string pair, OrderType type, decimal price, decimal amount);
		Task<Balance> GetInfoAsync();
		Balance GetInfo();
		Task<TradeInfo> GetTradesAsync(string pair, uint limit = 150);
		TradeInfo GetTrades(string pair, uint limit = 150);
		Task<PairsInfo> GetPairsAsync();
		PairsInfo GetPairs();
		Task<PairData> GetPairDataAsync(string pair);
		PairData GetPairData(string pair);
		Task<PairOrders> GetPairOrdersAsync(string pair, uint limit = 150);
		PairOrders GetPairOrders(string pair, uint limit = 150);
		Task<OrderDetails> GetActiveOrdersOfUserAsync(string pair);
		OrderDetails GetActiveOrdersOfUser(string pair);
	}
}