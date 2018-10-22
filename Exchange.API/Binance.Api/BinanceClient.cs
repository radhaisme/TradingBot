using Binance.Api.Models;
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

namespace Binance.Api
{
	public sealed class BinanceClient : ApiClient
	{
		private readonly IBinanceSettings _settings;
		private readonly int[] _limits = { 5, 10, 20, 50, 100, 500, 1000 };

		public BinanceClient()
		{
			_settings = new BinanceSettings();
		}

		#region Public API

		public async Task<TradePairsResponse> GetTradePairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "exchangeInfo"));
			var pairs = new List<TradePairResult>();

			foreach (dynamic item in content.symbols)
			{
				pairs.Add(new TradePairResult((string)item.baseAsset, (string)item.quoteAsset));
			}

			return new TradePairsResponse(pairs);
		}

		public async Task<MarketResponse> GetMarketAsync(MarketRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"ticker/price?symbol={request.Pair}"));

			return new MarketResponse { LastPrice = content.price };
		}

		public async Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			if (!_limits.Contains(request.Limit))
			{
				throw new ArgumentOutOfRangeException(nameof(request.Limit));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"depth?symbol={request.Pair}&limit={request.Limit}"));
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

		public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request)
		{
			var queryString = HttpHelper.QueryString(new Dictionary<string, string>
			{
				{ "symbol", request.Pair },
				{ "side", request.TradeType.ToString().ToUpper() },
				{ "type", request.OrderType.ToString().ToUpper() },
				{ "quantity", request.Amount.ToString(CultureInfo.InvariantCulture) },
				{ "price", request.Rate.ToString(CultureInfo.InvariantCulture) },
				{ "timeInForce", "GTC" },
				{ "timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() }
			}, true);
			var content = await MakePrivateCallAsync(HttpMethod.Post, "order", queryString);

			return new CreateOrderResponse((long)content.orderId);
		}

		public async Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{
			if (request.OrderId <= 0)
			{
				throw new ArgumentException(nameof(request.OrderId));
			}

			var queryString = HttpHelper.QueryString(new Dictionary<string, string>
			{
				{ "symbol", request.Pair },
				{ "orderId", request.OrderId.ToString() },
				{ "timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() }
			}, true);
			var content = await MakePrivateCallAsync(HttpMethod.Delete, "order", queryString);

			return new CancelOrderResponse((long)content.orderId);
		}

		public async Task<OpenOrdersResponse> GetOpenOrdersAsync(OpenOrdersRequest request)
		{
			var queryString = HttpHelper.QueryString(new Dictionary<string, string>
			{
				{ "symbol", request.Pair },
				{ "timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() }
			}, true);
			dynamic content = await MakePrivateCallAsync(HttpMethod.Get, "openOrders", queryString);
			var orders = new List<OrderResult>();

			foreach (dynamic item in content)
			{
				var order = new OrderResult((long)item.orderId)
				{
					Pair = item.symbol,
					TradeType = Enums.Parse<TradeType>((string)item.side, true),
					OrderType = Enums.Parse<OrderType>((string)item.type, true),
					Rate = item.price,
					Amount = item.origQty,
					CreatedAt = DateTimeOffset.FromUnixTimeSeconds((long)double.Parse((string)item.time))
				};
				orders.Add(order);
			}

			return new OpenOrdersResponse(orders);
		}

		#endregion

		#region Private methods

		private Task<dynamic> MakePrivateCallAsync(HttpMethod method, string url, string content)
		{
			SetHeaders(new Dictionary<string, string> { { "X-MBX-APIKEY", _settings.ApiKey } });
			string hash = HttpHelper.GetHash(new HMACSHA256(), _settings.Secret, content);
			content += $"&signature={hash}";

			return CallAsync<dynamic>(method, BuildUrl(_settings.PrivateUrl, url), new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
			{
				return;
			}

			var content = await HttpHelper.AcquireContentAsync<ErrorModel>(response);
			throw new HttpRequestException(content.Msg);
		}

		#endregion
	}

	public static class Interval
	{
		public const string OneM = "1m";
		public const string ThreeM = "3m";
		public const string FiveM = "5m";
		public const string FifteenM = "15m";
		public const string ThirteenM = "30m";
		public const string OneH = "1h";
		public const string TwoH = "2h";
		public const string FourH = "4h";
		public const string SixH = "6h";
		public const string EightH = "8h";
		public const string TwelveH = "12h";
		public const string OneD = "1d";
		public const string ThreeD = "3d";
		public const string OneW = "1w";
		public const string OneMn = "1M";
	}
}