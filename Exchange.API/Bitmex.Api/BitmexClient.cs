using Bitmex.Api.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Api;

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
			var order = new { nonce = DateTime.Now.ToString(CultureInfo.InvariantCulture) };
			dynamic content = await MakePrivateCallAsync(order, "orders");
			var orders = new List<OrderResult>();

			foreach (dynamic item in content)
			{

			}

			return new OpenOrdersResponse(orders);
		}

		#endregion

		#region Private method

		private Task<dynamic> MakePrivateCallAsync(object obj, string url)
		{
			//string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonHelper.ToJson(obj)));
			//SetHeaders(new Dictionary<string, string>
			//{
			//	{ "X-BFX-APIKEY", _settings.ApiKey },
			//	{ "X-BFX-PAYLOAD", base64 },
			//	{ "X-BFX-SIGNATURE", HttpHelper.GetHash(new HMACSHA384(), _settings.Secret, base64) }
			//});

			//return CallAsync<dynamic>(HttpMethod.Post, BuildUrl(_settings.PrivateUrl, url));

			return null;
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