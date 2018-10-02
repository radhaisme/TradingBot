using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Kucoin.Api.Models;
using TradingBot.Common;
using TradingBot.Core;

namespace Kucoin.Api
{
	public sealed class KucoinClient : ApiClient
	{
		private readonly IKucoinSettings _settings;

		public KucoinClient()
		{
			_settings = new KucoinSettings();
		}

		#region Public API

		public async Task<TradePairsResponse> GetTradePairsAsync()
		{
			var content = await CallAsync<ResponseModel>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "market/open/symbols"));
			var pairs = new List<TradePairResult>();

			foreach (dynamic item in content.Data)
			{
				if (!(bool)item.trading)
				{
					continue;
				}

				var pair = new TradePairResult();
				pair.BaseAsset = item.coinType;
				pair.QuoteAsset = item.coinTypePair;
				pairs.Add(pair);
			}

			return new TradePairsResponse(pairs);
		}

		public async Task<MarketResponse> GetMarketAsync(MarketRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<ResponseModel>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"{request.Pair}/open/tick"));

			if (!(bool)content.Data.trading)
			{
				return null;
			}

			dynamic data = content.Data;

			return new MarketResponse
			{
				LastPrice = data.lastDealPrice,
				AskPrice = data.buy,
				BidPrice = data.sell,
				Volume = data.vol,
				Low = ((decimal?)data.low).GetValueOrDefault(),
				High = ((decimal?)data.high).GetValueOrDefault()
			};
		}

		public async Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<ResponseModel>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"open/orders?symbol={request.Pair}&limit={request.Limit}"));
			dynamic data = content.Data;
			var asks = ((IEnumerable<dynamic>)data.BUY).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x[0], Volume = x[1] }).Where(x => x.Rate > 0);
			var bids = ((IEnumerable<dynamic>)data.SELL).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x[0], Volume = x[1] }).Where(x => x.Rate > 0);

			if (!asks.Any() || !bids.Any())
			{
				return new DepthResponse();
			}

			if (asks.Count() < bids.Count())
			{
				bids = bids.Take(asks.Count());
			}
			else if (asks.Count() > bids.Count())
			{
				asks = asks.Take(bids.Count());
			}

			return new DepthResponse(asks.ToList(), bids.ToList());
		}

		#endregion

		#region Private API

		public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request)
		{


			return new CreateOrderResponse(0);
		}

		public async Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{


			return new CancelOrderResponse(0);
		}

		public async Task<OpenOrdersResponse> GetOpenOrdersAsync(OpenOrdersRequest request)
		{
			//var queryString = HttpHelper.QueryString(new Dictionary<string, string>
			//{
			//	{ "symbol", request.Pair },
			//	{ "timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() }
			//}, true);
			//dynamic content = await MakePrivateCallAsync(HttpMethod.Get, "openOrders", queryString);
			var orders = new List<OrderResult>();

			//foreach (dynamic item in content)
			//{

			//}

			return new OpenOrdersResponse(orders);
		}

		#endregion

		protected override async void HandleError(HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
			{
				return;
			}

			var content = await HttpHelper.AcquireContentAsync<ResponseModel>(response);
			string message = content.Msg;

			if (String.IsNullOrEmpty(content.Msg))
			{
				message = "Some error occurred.";
			}

			throw new HttpRequestException(message);
		}
	}
}