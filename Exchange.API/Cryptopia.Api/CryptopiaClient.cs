using Cryptopia.Api.Models;
using EnumsNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Api;
using TradingBot.Api.Helpers;

namespace Cryptopia.Api
{
	public sealed class CryptopiaClient : ApiClient
	{
		private readonly ICryptopiaSettings _settings;

		public CryptopiaClient()
		{
			_settings = new CryptopiaSettings();
		}

		#region Public API

		public async Task<TradePairsResponse> GetTradePairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "GetTradePairs"));
			var pairs = new List<TradePairResult>();

			foreach (dynamic item in content.Data)
			{
				var pair = new TradePairResult
				{
					BaseAssetName = item.Currency,
					BaseAsset = item.Symbol,
					QuoteAssetName = item.BaseCurrency,
					QuoteAsset = item.BaseSymbol,
					MaxOrderSize = item.MaximumTrade,
					MinOrderSize = item.MinimumTrade
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

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"GetMarket/{request.Pair}"));
			var response = new MarketResponse
			{
				AskPrice = content.Data.AskPrice,
				BidPrice = content.Data.BidPrice,
				High = content.Data.High,
				Low = content.Data.Low,
				LastPrice = content.Data.LastPrice,
				Volume = content.Data.Volume
			};

			return response;
		}

		public async Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"GetMarketOrders/{request.Pair}/{request.Limit}"));
			var asks = ((IEnumerable<dynamic>)content.Data.Buy).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x.Price, Volume = x.Volume }).Where(x => x.Rate > 0);
			var bids = ((IEnumerable<dynamic>)content.Data.Sell).Take(request.Limit).Select(x => new OrderInBookResult { Rate = x.Price, Volume = x.Volume }).Where(x => x.Rate > 0);

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
			//TODO: Place an order validation to here
			var content = await MakePrivateCallAsync(BuildUrl(_settings.PrivateUrl, "SubmitTrade"), request);
			
			return new CreateOrderResponse((long)content.Data.OrderId);
		}

		public async Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{
			//TODO: Place an order validation to here
			await MakePrivateCallAsync(BuildUrl(_settings.PrivateUrl, "CancelTrade"), request);
			
			return new CancelOrderResponse(request.OrderId);
		}

		public async Task<OpenOrdersResponse> GetOpenOrdersAsync(OpenOrdersRequest request)
		{
			dynamic content = await MakePrivateCallAsync(BuildUrl(_settings.PrivateUrl, "GetOpenOrders"), request);
			var orders = new List<OrderResult>();

			foreach (dynamic item in content.Data)
			{
				var order = new OrderResult((long)item.OrderId)
				{
					Pair = item.Market,
					TradeType = Enums.Parse<TradeType>((string)item.Type, true),
					Rate = item.Rate,
					Amount = item.Amount,
					CreatedAt = DateTimeOffset.Parse((string)item.TimeStamp)
				};
				orders.Add(order);
			}

			return new OpenOrdersResponse(orders);
		}

		#endregion

		#region Private methods

		private Task<dynamic> MakePrivateCallAsync(Uri uri, object request)
		{
			var nonce = Guid.NewGuid().ToString("N");
			string json = JsonConvert.SerializeObject(request);
			string contentBase64String = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(json)));
			var rawData = String.Concat(_settings.ApiKey, "POST", uri.ToString(), nonce, contentBase64String);
			string signature = HttpHelper.GetHash(HMACSHA256.Create(), _settings.Secret, rawData);
			SetHeaders(new Dictionary<string, string> { { "amx", $"{_settings.ApiKey}:{signature}:{nonce}" } });
			
			return CallAsync<dynamic>(HttpMethod.Post, uri, new StringContent(json, Encoding.UTF8, "application/json"));
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			var content = await HttpHelper.AcquireContentAsync<ResponseModel>(response);

			if (!String.IsNullOrEmpty(content.Error))
			{
				throw new HttpRequestException(content.Error);
			}
		}

		#endregion
	}
}