using Huobi.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Huobi.Api
{
	public sealed class HuobiClient : ApiClient
	{
		private readonly IHuobiSettings _settings;

		public HuobiClient()
		{
			_settings = new HuobiSettings();
		}

		#region Public API

		public async Task<TradePairsResponse> GetTradePairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "v1/common/symbols"));
			var pairs = new List<TradePairResult>();

			foreach (dynamic item in content.data)
			{
				var pair = new TradePairResult();
				pair.BaseAsset = ((string)item["base-currency"]).ToUpper();
				pair.QuoteAsset = ((string)item["quote-currency"]).ToUpper();
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

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"market/detail/merged?symbol={request.Pair}"));

			return new MarketResponse
			{
				LastPrice = content.tick.close,
				AskPrice = content.tick.ask[0],
				BidPrice = content.tick.bid[0],
				Volume = content.tick.vol,
				High = content.tick.high,
				Low = content.tick.low
			};
		}

		public async Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"market/depth?symbol={request.Pair}&type=step1"));
			var asks = ((IEnumerable<dynamic>)content.tick.asks).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x[0], Volume = x[1] }).Where(x => x.Rate > 0);
			var bids = ((IEnumerable<dynamic>)content.tick.bids).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x[0], Volume = x[1] }).Where(x => x.Rate > 0);

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

		//public async Task<IReadOnlyCollection<PairDetailResponse>> GetPairsDetails(params string[] pairs)
		//{
		//	var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairsDetailsAsync());
		//	var details = new List<PairDetailResponse>();

		//	foreach (dynamic item in content.data)
		//	{
		//		var dto = new PairDetailResponse();
		//		dto.LastPrice = item.close;
		//		//detail.Low = item.low;
		//		//detail.High = item.high;
		//		//detail.Volume = item.vol;
		//		details.Add(dto);
		//	}

		//	return new ReadOnlyCollection<PairDetailResponse>(details);
		//}
	}
}