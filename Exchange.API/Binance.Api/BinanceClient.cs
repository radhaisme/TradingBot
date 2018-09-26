﻿using System;
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

namespace Binance.Api
{
	public sealed class BinanceClient : ApiClient, IApiClient
	{
		private readonly IBinanceSettings _settings;
		private readonly uint[] _limits = { 5, 10, 20, 50, 100, 500, 1000 };

		public BinanceClient()
		{
			_settings = new BinanceSettings();
		}

		public ExchangeType Type => ExchangeType.Binance;

		public async Task<PairResponse> GetPairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "exchangeInfo"));
			var pairs = new List<PairDto>();

			foreach (dynamic item in content.symbols)
			{
				var dto = new PairDto();
				dto.BaseAsset = item.baseAsset;
				dto.QuoteAsset = item.quoteAsset;
				pairs.Add(dto);
			}

			return new PairResponse(pairs);
		}

		public async Task<PairDetailResponse> GetPairDetailAsync(PairDetailRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"ticker/price?symbol={request.Pair}"));
			var response = new PairDetailResponse();
			response.LastPrice = content.price;

			return response;
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
			var response = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.asks).Take((int)request.Limit), ((IEnumerable<dynamic>)content.bids).Take((int)request.Limit), item => new BookOrderDto { Price = item[0], Amount = item[1] });

			return response;
		}

		public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request)
		{
			var queryString = HttpHelper.QueryString(new Dictionary<string, string>
			{
				{ "symbol", request.Pair },
				{ "side", request.Side.ToString().ToUpper() },
				{ "type", request.Type.ToString().ToUpper() },
				{ "quantity", request.Amount.ToString(CultureInfo.InvariantCulture) },
				{ "price", request.Price.ToString(CultureInfo.InvariantCulture) },
				{ "timeInForce", "GTC" },
				{ "timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() }
			}, true);
			var content = await MakePrivateCallAsync(HttpMethod.Post, "order", queryString);
			var response = new CreateOrderResponse();
			response.OrderId = content.orderId;

			return response;
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
			var response = new CancelOrderResponse();
			response.OrderId = content.orderId;

			return response;
		}

		public async Task<OrderResponse> GetOrdersAsync(OrderRequest request)
		{
			var queryString = HttpHelper.QueryString(new Dictionary<string, string>
			{
				{ "symbol", request.Pair },
				{ "timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() }
			}, true);
			dynamic content = await MakePrivateCallAsync(HttpMethod.Get, "openOrders", queryString);
			var response = new OrderResponse();

			foreach (dynamic item in content)
			{
				
			}

			return response;
		}

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