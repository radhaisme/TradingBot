using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace Bitfinex.Api
{
	public sealed class BitfinexClient : ApiClient
	{
		private readonly IBitfinexSettings _settings;

		public BitfinexClient()
		{
			_settings = new BitfinexSettings();
		}

		public ExchangeType Type => ExchangeType.Bitfinex;

		#region Public API

		public async Task<PairResponse> GetPairsAsync()
		{
			var content = await CallAsync<string[]>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "symbols"));
			var pairs = new Dictionary<string, TradePair>(content.Length);

			foreach (var item in content)
			{
				var dto = new TradePair();
				dto.BaseAsset = item.Substring(0, item.Length - 3);
				dto.QuoteAsset = item.Substring(item.Length - 3, 3);
				pairs.Add(item, dto);
			}

			return new PairResponse(pairs.Values.ToList());
		}

		public async Task<PairDetailResponse> GetPairDetailAsync(PairDetailRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"pubticker/{request.Pair}"));
			var response = new PairDetailResponse();
			response.LastPrice = content.last_price;
			response.Ask = content.ask;
			response.Bid = content.bid;
			response.Volume = content.volume;
			response.High = content.high;
			response.Low = content.low;

			return response;
		}

		public async Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			//if (String.IsNullOrEmpty(request.Pair))
			//{
			//	throw new ArgumentNullException(nameof(request.Pair));
			//}

			//var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"book/{request.Pair}?limit_bids={request.Limit}&limit_asks={request.Limit}"));
			//var response = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.asks).Take((int)request.Limit), ((IEnumerable<dynamic>)content.bids).Take((int)request.Limit), item => new BookOrderDto { Price = item.price, Amount = item.amount });

			//return response;

			return null;
		}

		#endregion

		#region Private API

		public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request)
		{
			var order = new
			{
				symbol = request.Pair,
				amount = request.Amount,
				price = request.Rate,
				side = request.TradeType.ToString().ToLower(),
				type = GetOrderType(request.Type),
				ocoorder = false //TODO: Unsupported an ocoorder orders
			};
			var content = await MakePrivateCallAsync(order, "order/new");
			var response = new CreateOrderResponse();
			response.OrderId = content.order_id;

			return response;
		}

		public async Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{
			var order = new { order_id = request.OrderId };
			var content = await MakePrivateCallAsync(order, "order/cancel");
			var dto = new CancelOrderResponse();
			dto.OrderId = content.order_id;

			return dto;
		}

		public async Task<OrderResponse> GetOrdersAsync(OrderRequest request)
		{
			var order = new { nonce = DateTime.Now.ToString(CultureInfo.InvariantCulture) };
			dynamic content = await MakePrivateCallAsync(order, "orders");
			var response = new OrderResponse();

			foreach (dynamic item in content)
			{

			}

			return response;
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