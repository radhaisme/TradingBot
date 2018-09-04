using System.Threading.Tasks;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace Bitmex.Api
{
	public sealed class BitmexClient : ApiClient, IApiClient
	{
		public ExchangeType Type => ExchangeType.Bitmex;

		public Task<PairResponse> GetPairsAsync()
		{
			throw new System.NotImplementedException();
		}

		public Task<PairDetailResponse> GetPairDetailAsync(PairDetailRequest request)
		{
			throw new System.NotImplementedException();
		}

		public Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			throw new System.NotImplementedException();
		}

		public Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request)
		{
			throw new System.NotImplementedException();
		}

		public Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{
			throw new System.NotImplementedException();
		}
	}
}