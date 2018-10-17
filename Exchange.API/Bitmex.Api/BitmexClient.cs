using Bitmex.Api.Models;
using EnumsNET;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Api;
using TradingBot.Api.Helpers;

namespace Bitmex.Api
{
	public sealed class BitmexClient : ApiClient
	{
		private readonly IBitmexSettings _settings;

		public BitmexClient()
		{
			_settings = new BitmexSettings();
		}

		#region Public API

		public async Task<TradePairsResponse> GetTradePairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "instrument/active"));
			var pairs = new List<TradePairResult>();

			foreach (var item in content)
			{
				var symbol = (string)item.symbol;
				var pair = new TradePairResult
				{
					Symbol = symbol,
					QuoteAsset = (string)item.underlying,
					BaseAsset = symbol.Substring(3)
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

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"trade/{request.Pair}"));
			var response = new MarketResponse();
			//response.LastPrice = content.last_price;
			//response.Ask = content.ask;
			//response.Bid = content.bid;
			//response.Volume = content.volume;
			//response.High = content.high;
			//response.Low = content.low;

			return response;
		}

		public async Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			//var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"book/{request.Pair}?limit_bids={request.Limit}&limit_asks={request.Limit}"));
			//var response = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.asks).Take((int)request.Limit), ((IEnumerable<dynamic>)content.bids).Take((int)request.Limit), item => new BookOrderDto { Price = item.price, Amount = item.amount });

			//return response;

			return new DepthResponse();
		}

		#endregion

		#region Private API

		public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var order = new
			{
				symbol = request.Pair,
				side = request.TradeType.ToString(),
				orderQty = request.Amount,
				price = request.Rate,
				ordType = request.OrderType.ToString()
			};
			string json = JsonHelper.ToJson(order);
			dynamic content = await MakePrivateCallAsync(HttpMethod.Post, "order", json);
			
			return new CreateOrderResponse(Guid.Parse((string)content.orderID));
		}

		public async Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{
			if (request.OrderId == Guid.Empty)
			{
				throw new ArgumentException(nameof(request.OrderId));
			}

			dynamic content = await MakePrivateCallAsync(HttpMethod.Delete, "order", JsonHelper.ToJson(new { orderID = request.OrderId }));

			return new CancelOrderResponse(Guid.Parse((string)content.orderID));
		}

		public async Task<OpenOrdersResponse> GetOpenOrdersAsync(OpenOrdersRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "symbol", request.Pair } }, true);
			dynamic content = await MakePrivateCallAsync(HttpMethod.Get, $"order?{queryString}&filter=%7B%22open%22%3A%20true%7D", String.Empty);
			var orders = new List<OrderResult>();

			foreach (dynamic item in content)
			{
				var order = new OrderResult((string)item.orderID)
				{
					Pair = item.symbol,
					TradeType = Enums.Parse<TradeType>((string)item.side, true),
					Rate = item.price,
					Amount = item.orderQty,
					CreatedAt = DateTimeOffset.Parse((string)item.timestamp)
				};
				orders.Add(order);
			}

			return new OpenOrdersResponse(orders);
		}

		#endregion

		#region Private method

		private Task<dynamic> MakePrivateCallAsync(HttpMethod method, string url, string content)
		{
			StringContent body = null;

			if (!method.Equals(HttpMethod.Get))
			{
				body = new StringContent(content, Encoding.UTF8, "application/json");
			}

			var uri = BuildUrl(_settings.PrivateUrl, url);
			string expires = GetExpiresArg();
			string message = method + uri.PathAndQuery + expires + content;
			SetHeaders(new Dictionary<string, string>
			{
				{ "api-expires", expires },
				{ "api-key", _settings.ApiKey },
				{ "api-signature", HttpHelper.GetHash(new HMACSHA256(), _settings.Secret, message) }
			});
			
			return CallAsync<dynamic>(method, uri, body);
		}

		private string GetExpiresArg()
		{
			var timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			var expires = (timestamp + 60).ToString();

			return expires;
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(response);
			
			if (!response.IsSuccessStatusCode && content.error != null)
			{
				throw new HttpRequestException((string)content.error.message);
			}
		}

		#endregion
	}
}