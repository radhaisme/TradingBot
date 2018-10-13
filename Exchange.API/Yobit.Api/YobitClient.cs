using EnumsNET;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Api;
using TradingBot.Api.Helpers;
using Yobit.Api.Models;

namespace Yobit.Api
{
	public sealed class YobitClient : ApiClient
	{
		private readonly IYobitSettings _settings;

		public YobitClient()
		{
			_settings = new YobitSettings();
		}

		#region Public API

		public async Task<TradePairsResponse> GetTradePairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "info?ignore_invalid=1"));
			var pairs = new List<TradePairResult>();

			foreach (dynamic key in content.pairs)
			{
				dynamic pair = content.pairs[(string)key.Name];

				if ((bool)pair.hidden)
				{
					continue;
				}

				string[] assets = ((string)key.Name).Split('_');
				var item = new TradePairResult
				{
					BaseAsset = assets[0],
					QuoteAsset = assets[1]
				};
				pairs.Add(item);
			}

			return new TradePairsResponse(pairs);
		}

		public async Task<MarketResponse> GetMarketAsync(MarketRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"ticker/{request.Pair}?ignore_invalid=1"));
			dynamic data = content[request.Pair];

			return new MarketResponse
			{
				LastPrice = data.last,
				Volume = data.vol,
				AskPrice = data.buy,
				BidPrice = data.sell,
				High = data.high,
				Low = data.low
			};
		}

		public async Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"depth/{request.Pair}?limit={request.Limit}&ignore_invalid=1"));
			dynamic data = content[request.Pair];
			var asks = ((IEnumerable<dynamic>)data.asks).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x[0], Volume = x[1] }).Where(x => x.Rate > 0);
			var bids = ((IEnumerable<dynamic>)data.bids).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x[0], Volume = x[1] }).Where(x => x.Rate > 0);

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
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			string queryString = HttpHelper.QueryString(new Dictionary<string, string>
			{
				{"method", "Order"},
				{"pair", request.Pair},
				{"type", request.TradeType.ToString()},
				{"rate", request.Rate.ToString(CultureInfo.InvariantCulture)},
				{"amount", request.Amount.ToString(CultureInfo.InvariantCulture)},
				{"nonce", GenerateNonce(_settings.CreatedAt)}
			}, true);
			var content = await MakePrivateCallAsync(queryString);

			return new CreateOrderResponse((long)content.order_id);
		}

		public async Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{
			if (request.OrderId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(request.OrderId));
			}

			string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "CancelOrderRequest" }, { "order_id", request.OrderId.ToString() }, { "nonce", GenerateNonce(_settings.CreatedAt) } }, true);
			var content = await MakePrivateCallAsync(queryString);

			return new CancelOrderResponse((long)content.@return.order_id);
		}

		public async Task<OpenOrdersResponse> GetOpenOrdersAsync(OpenOrdersRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "ActiveOrders" }, { "pair", request.Pair }, { "nonce", GenerateNonce(_settings.CreatedAt) } }, true);
			dynamic content = await MakePrivateCallAsync(queryString);
			var orders = new List<OrderResult>();

			if (content.@return != null)
			{
				foreach (dynamic key in content.@return)
				{
					dynamic data = content.@return[key];
					var order = new OrderResult((long)key)
					{
						Pair = data.pair,
						TradeType = Enums.Parse<TradeType>((string)data.type, true),
						Rate = data.rate,
						Amount = data.amount,
						CreatedAt = DateTimeOffset.FromUnixTimeSeconds((long)double.Parse((string)data.timestamp_created))
					};
					orders.Add(order);
				}
			}

			return new OpenOrdersResponse(orders);
		}

		#endregion

		#region Private method

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
					"Sign", HttpHelper.GetHash(new HMACSHA512(), _settings.Secret, content).ToLower()
				}
			});

			return CallAsync<dynamic>(HttpMethod.Post, BuildUrl(_settings.PrivateUrl, "?" + content), new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));
		}

		private string GenerateNonce(DateTimeOffset date)
		{
			return ((DateTime.UtcNow - date).TotalSeconds).ToString(CultureInfo.InvariantCulture);
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(response);

			if (!(bool)content.success && !String.IsNullOrEmpty((string)content.error))
			{
				throw new HttpRequestException(content.Error);
			}
		}

		#endregion
	}
}