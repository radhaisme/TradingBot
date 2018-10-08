using Bitfinex.Api.Models;
using EnumsNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Api;
using TradingBot.Api.Helpers;

namespace Bitfinex.Api
{
	public sealed class BitfinexClient : ApiClient
	{
		private readonly IBitfinexSettings _settings;

		public BitfinexClient()
		{
			_settings = new BitfinexSettings();
		}

		#region Public API

		public async Task<TradePairsResponse> GetTradePairsAsync()
		{
			var content = await CallAsync<string[]>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "symbols"));
			var pairs = new Dictionary<string, TradePairResult>(content.Length);

			foreach (var item in content)
			{
				var pair = new TradePairResult
				{
					BaseAsset = item.Substring(0, item.Length - 3),
					QuoteAsset = item.Substring(item.Length - 3, 3)
				};
				pairs.Add(item, pair);
			}

			return new TradePairsResponse(pairs.Values.ToList());
		}

		public async Task<MarketResponse> GetMarketAsync(MarketRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"pubticker/{request.Pair}"));
			
			return new MarketResponse
			{
				LastPrice = content.last_price,
				AskPrice = content.ask,
				BidPrice = content.bid,
				Volume = content.volume,
				High = content.high,
				Low = content.low
			};
		}

		public async Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"book/{request.Pair}?limit_bids={request.Limit}&limit_asks={request.Limit}"));
			var asks = ((IEnumerable<dynamic>)content.asks).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x.price, Volume = x.amount }).Where(x => x.Rate > 0);
			var bids = ((IEnumerable<dynamic>)content.bids).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x.price, Volume = x.amount }).Where(x => x.Rate > 0);

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
			var content = await MakePrivateCallAsync(request, "order/new");

			return new CreateOrderResponse((long)content.order_id);
		}

		public async Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{
			var content = await MakePrivateCallAsync(request, "order/cancel");

			return new CancelOrderResponse((long)content.order_id);
		}

		public async Task<OpenOrdersResponse> GetOpenOrdersAsync(OpenOrdersRequest request)
		{
			dynamic content = await MakePrivateCallAsync(request, "orders");
			var orders = new List<OrderResult>();

			foreach (dynamic item in content)
			{
				var order = new OrderResult((long)item.id)
				{
					Pair = item.symbol,
					TradeType = Enums.Parse<TradeType>((string)item.side, true),
					Rate = item.price,
					Amount = item.original_amount,
					CreatedAt = DateTimeOffset.FromUnixTimeSeconds((long)double.Parse((string)item.timestamp))
				};
				orders.Add(order);
			}

			return new OpenOrdersResponse(orders);
		}

		#endregion

		#region Private method

		private Task<dynamic> MakePrivateCallAsync(object obj, string url)
		{
			string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonHelper.ToJson(obj)));
			SetHeaders(new Dictionary<string, string>
			{
				{ "X-BFX-APIKEY", _settings.ApiKey },
				{ "X-BFX-PAYLOAD", base64 },
				{ "X-BFX-SIGNATURE", HttpHelper.GetHash(new HMACSHA384(), _settings.Secret, base64) }
			});

			return CallAsync<dynamic>(HttpMethod.Post, BuildUrl(_settings.PrivateUrl, url));
		}

		private string GetOrderType(OrderType type)
		{
			return $"exchange {type.ToString().ToLower()}";
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
			{
				return;
			}

			var content = await HttpHelper.AcquireContentAsync<ErrorModel>(response);
			throw new HttpRequestException(content.Message);
		}

		#endregion
	}
}