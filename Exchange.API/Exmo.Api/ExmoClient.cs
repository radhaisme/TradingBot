using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Exmo.Api.Models;
using TradingBot.Common;
using TradingBot.Core;

namespace Exmo.Api
{
	public sealed class ExmoClient : ApiClient
	{
		private readonly IExmoSettings _settings;

		public ExmoClient()
		{
			_settings = new ExmoSettings();
		}

		#region Public API

		public async Task<TradePairsResponse> GetTradePairsAsync()
		{
			var content = await CallAsync<Dictionary<string, dynamic>>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "pair_settings"));
			var pairs = new List<TradePairResult>();

			foreach (string key in content.Keys)
			{
				var pair = new TradePairResult();
				string[] assets = key.Split('_');
				pair.BaseAsset = assets[0];
				pair.QuoteAsset = assets[1];
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

			var content = await CallAsync<Dictionary<string, dynamic>>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "ticker"));
			dynamic item = content[request.Pair];

			return new MarketResponse
			{
				LastPrice = item.last_trade,
				High = item.high,
				Low = item.low,
				Volume = item.vol,
				AskPrice = item.buy_price,
				BidPrice = item.sell_price
			};
		}

		public async Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"order_book?pair={request.Pair}&limit={request.Limit}"));
			var asks = ((IEnumerable<dynamic>)content[request.Pair].ask).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x[0], Volume = x[1] }).Where(x => x.Rate > 0);
			var bids = ((IEnumerable<dynamic>)content[request.Pair].bid).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x[0], Volume = x[1] }).Where(x => x.Rate > 0);

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

		public Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request)
		{
			//HMACSHA512

			return null;
		}

		public Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{


			return null;
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

		#region Private methods

		private Task<dynamic> MakePrivateCallAsync(string content)
		{
			if (String.IsNullOrEmpty(_settings.PrivateUrl))
			{
				throw new ArgumentNullException(nameof(_settings.PrivateUrl));
			}

			if (String.IsNullOrEmpty(_settings.ApiKey))
			{
				throw new ArgumentNullException(nameof(_settings.ApiKey));
			}

			if (String.IsNullOrEmpty(_settings.Secret))
			{
				throw new ArgumentNullException(nameof(_settings.Secret));
			}

			SetHeaders(new Dictionary<string, string>
			{
				{"Key", _settings.ApiKey},
				{
					"Sign", BitConverter.ToString(new HMACSHA512(Encoding.UTF8.GetBytes(_settings.Secret)).ComputeHash(Encoding.UTF8.GetBytes(content))).Replace("-", "").ToLower()
				}
			});

			return CallAsync<dynamic>(HttpMethod.Post, BuildUrl(_settings.PrivateUrl, content), new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
			{
				return;
			}

			var content = await HttpHelper.AcquireContentAsync<ErrorModel>(response);
			throw new HttpRequestException(content.Error);
		}

		#endregion
	}
}