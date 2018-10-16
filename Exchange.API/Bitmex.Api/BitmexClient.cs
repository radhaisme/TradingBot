using Bitmex.Api.Models;
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
			//var order = new
			//{
			//	symbol = request.Pair,
			//	amount = request.Amount,
			//	price = request.Rate,
			//	side = request.TradeType.ToString().ToLower(),
			//	type = GetOrderType(request.Type),
			//	ocoorder = false //TODO: Unsupported an ocoorder orders
			//};
			//var content = await MakePrivateCallAsync(order, "order/new");
			var response = new CreateOrderResponse(0);
			//response.OrderId = content.order_id;

			return response;
		}

		public async Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{
			//var order = new { order_id = request.OrderId };
			//var content = await MakePrivateCallAsync(order, "order/cancel");
			var response = new CancelOrderResponse(0);
			//dto.OrderId = content.order_id;

			return response;
		}

		public async Task<OpenOrdersResponse> GetOpenOrdersAsync(OpenOrdersRequest request)
		{
			string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "symbol", request.Pair } });
			dynamic content = await MakePrivateCallAsync(HttpMethod.Get, "order" + queryString, String.Empty);
			var orders = new List<OrderResult>();

			//foreach (dynamic item in content)
			//{
			//	if (!(bool) item.IsOpen)
			//	{
			//		continue;
			//	}

			//	var order = new OrderResult((string) item.Symbol)
			//	{
			//		Rate = item.MarkPrice,
			//		Amount = item.CurrentQty,
			//		CreatedAt = item.TimeStamp
			//	};
			//	orders.Add(order);
			//}

			return new OpenOrdersResponse(orders);
		}

		#endregion

		#region Private method

		private Task<dynamic> MakePrivateCallAsync(HttpMethod method, string url, string content)
		{
			var uri = BuildUrl(_settings.PrivateUrl, url);
			string expires = GetExpiresArg();
			string message = method + uri.PathAndQuery + expires + content;
			string signature = HttpHelper.GetHash(new HMACSHA256(), _settings.Secret, message); //ByteArrayToString(hmacsha256(Encoding.UTF8.GetBytes(_settings.Secret),
				//Encoding.UTF8.GetBytes(message))); //HttpHelper.GetHash(new HMACSHA256(), _settings.Secret, message);
			SetHeaders(new Dictionary<string, string>
			{
				{ "api-expires", expires },
				{ "api-key", _settings.ApiKey },
				{ "api-signature", signature }
			});
			//new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")
			
			return CallAsync<dynamic>(method, uri);
		}

		public static string ByteArrayToString(byte[] arr)
		{
			StringBuilder hex = new StringBuilder(arr.Length * 2);
			foreach (byte b in arr)
				hex.AppendFormat("{0:x2}", b);
			return hex.ToString();
		}

		public byte[] hmacsha256(byte[] keyByte, byte[] messageBytes)
		{
			using (var hash = new HMACSHA256(keyByte))
			{
				return hash.ComputeHash(messageBytes);
			}
		}

		private string GetExpiresArg()
		{
			long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			string expires = (timestamp + 60).ToString();

			return (expires);
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			//if (response.IsSuccessStatusCode)
			//{
			//	return;
			//}

			//var content = await HttpHelper.AcquireContentAsync<ErrorModel>(response);
			//throw new HttpRequestException(content.Message);
		}

		#endregion
	}
}