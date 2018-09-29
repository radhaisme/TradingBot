using Okex.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Enums;

namespace Okex.Api
{
	public sealed class OkexClient : ApiClient
	{
		private readonly IOkexSettings _settings;

		public OkexClient()
		{
			_settings = new OkexSettings();
		}

		public ExchangeType Type => ExchangeType.Okex;

		#region Public API

		public async Task<TradePairsResponse> GetTradePairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "tickers.do"));
			var pairs = new List<TradePairResult>();

			foreach (dynamic item in content.tickers)
			{
				string[] assets = ((string)item.symbol).Split('_');
				var pair = new TradePairResult
				{
					BaseAsset = assets[0].ToUpper(),
					QuoteAsset = assets[1].ToUpper()
				};
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

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"ticker.do?symbol={request.Pair}"));

			return new MarketResponse
			{
				LastPrice = content.ticker.last,
				AskPrice = content.ticker.buy,
				BidPrice = content.ticker.sell,
				High = content.ticker.high,
				Low = content.ticker.low,
				Volume = content.ticker.vol
			};
		}

		public async Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"depth.do?symbol={request.Pair}&size={request.Limit}"));
			var asks = ((IEnumerable<dynamic>)content.asks).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x[0], Volume = x[1] }).Where(x => x.Rate > 0);
			var bids = ((IEnumerable<dynamic>)content.bids).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x[0], Volume = x[1] }).Where(x => x.Rate > 0);

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


			return null;
		}

		public Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{


			return null;
		}

		public async Task<OpenOrdersResponse> GetOpenOrdersAsync(OpenOrdersRequest request)
		{
			var queryString = HttpHelper.QueryString(new Dictionary<string, string>
			{
				{ "symbol", request.Pair },
				{ "timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() }
			}, true);
			//dynamic content = await MakePrivateCallAsync(HttpMethod.Get, "openOrders", queryString);
			var response = new OpenOrdersResponse();

			//foreach (dynamic item in content)
			//{
				
			//}

			return response;
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
			var content = await HttpHelper.AcquireContentAsync<ErrorModel>(response);

			if (content.ErrorCode > 0)
			{
				string message = content.GetMessage();
				throw new HttpRequestException(message);
			}
		}

		#endregion
	}
}