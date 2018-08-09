using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace Cryptopia.Api
{
	public sealed class CryptopiaClient : ApiClient, IApiClient
	{
		private readonly ICryptopiaSettings _settings;

		public CryptopiaClient()
		{
			_settings = new CryptopiaSettings();
		}

		public ExchangeType Type => _settings.Type;

		public async Task<PairResponse> GetPairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "GetTradePairs"));
			var pairs = new List<PairDto>();

			foreach (dynamic item in content.Data)
			{
				var dto = new PairDto();
				dto.BaseAssetName = item.Currency;
				dto.BaseAsset = item.Symbol;
				dto.QuoteAssetName = item.BaseCurrency;
				dto.QuoteAsset = item.BaseSymbol;
				dto.MaxOrderSize = item.MaximumTrade;
				dto.MinOrderSize = item.MinimumTrade;
				pairs.Add(dto);
			}

			return new PairResponse(pairs);
		}

		public async Task<PairDetailResponse> GetPairDetailAsync(PairDetailRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"GetMarket/{request.Pair}"));
			var response = new PairDetailResponse();
			response.Ask = content.Data.AskPrice;
			response.Bid = content.Data.BidPrice;
			response.High = content.Data.High;
			response.Low = content.Data.Low;
			response.LastPrice = content.Data.LastPrice;
			response.Volume = content.Data.Volume;

			return response;
		}

		public async Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"GetMarketOrders/{request.Pair}/{request.Limit}"));
			var response = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.Data.Buy).Take((int)request.Limit), ((IEnumerable<dynamic>)content.Data.Sell).Take((int)request.Limit), item => new BookOrderDto { Price = item.Price, Amount = item.Volume });

			return response;
		}

		public Task<CreateOrderResponse> CreateOrderAsync(OrderRequest request)
		{
			//HMACSHA256

			return null;
		}

		public Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{


			return null;
		}

		#region Private methods

		private Task<dynamic> MakePrivateCallAsync(string url)
		{


			return CallAsync<dynamic>(HttpMethod.Post, BuildUrl(_settings.PrivateUrl, url));
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			var content = await HttpHelper.AcquireContentAsync<ResponseModel>(response);

			if (!String.IsNullOrEmpty(content.Error))
			{
				throw new HttpRequestException(content.Error);
			}
		}

		#endregion
	}
}