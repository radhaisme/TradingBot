using Cryptopia.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;
using CancelOrderRequest = Cryptopia.Api.Models.CancelOrderRequest;
using CancelOrderResponse = Cryptopia.Api.Models.CancelOrderResponse;
using CreateOrderRequest = Cryptopia.Api.Models.CreateOrderRequest;
using CreateOrderResponse = Cryptopia.Api.Models.CreateOrderResponse;
using DepthRequest = Cryptopia.Api.Models.DepthRequest;
using DepthResponse = Cryptopia.Api.Models.DepthResponse;

namespace Cryptopia.Api
{
	public sealed class CryptopiaClient : ApiClient
	{
		private readonly ICryptopiaSettings _settings;

		public CryptopiaClient()
		{
			_settings = new CryptopiaSettings();
		}

		public ExchangeType Type => ExchangeType.Cryptopia;

		#region Public API

		public async Task<TradePairsResponse> GetTradePairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "GetTradePairs"));
			var pairs = new List<TradePair>();

			foreach (dynamic item in content.Data)
			{
				var pair = new TradePair
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
			var asks = ((IEnumerable<dynamic>)content.Data.Buy).Take(request.Limit).Select(x => new OrderInBook { Rate = x.Price, Volume = x.Volume }).Where(x => x.Rate > 0);
			var bids = ((IEnumerable<dynamic>)content.Data.Sell).Take(request.Limit).Select(x => new OrderInBook { Rate = x.Price, Volume = x.Volume }).Where(x => x.Rate > 0);

			if (!asks.Any() || !bids.Any())
			{
				return new DepthResponse();
			}

			if (asks.Count() < bids.Count())
			{
				bids = bids.Take(asks.Count());
			}
			else
			{
				asks = asks.Take(bids.Count());
			}

			return new DepthResponse(asks.ToList(), bids.ToList());
		}

		#endregion

		#region Private API

		public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request)
		{
			var nonce = Guid.NewGuid().ToString("N");
			//var order = new { Market = request.Pair, Type = request.TradeType, request.Rate, request.Volume };

			string hash = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(String.Empty)));
			Uri uri = BuildUrl(_settings.PrivateUrl, "SubmitTrade");
			var raw = String.Concat(_settings.ApiKey, "POST", uri.ToString(), nonce, hash);

			HttpHelper.GetHash(HMACSHA256.Create(), _settings.Secret, String.Empty);

			//SetHeaders(new Dictionary<string, string> { { "amx", $"{_settings.ApiKey}:{0}:{nonce}" } });
			//var content = await CallAsync<dynamic>(HttpMethod.Post, uri, new StringContent(String.Empty));



			var response = new CreateOrderResponse();

			return response;
		}

		public Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{


			return null;
		}

		#endregion

		#region Private methods

		private async Task<dynamic> MakePrivateCallAsync(string url)
		{
			var nonce = Guid.NewGuid().ToString("N");
			string hash = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(String.Empty)));
			Uri uri = BuildUrl(_settings.PrivateUrl, "SubmitTrade");
			var rawData = String.Concat(_settings.ApiKey, "POST", uri.ToString(), nonce, hash);
			string signature = HttpHelper.GetHash(HMACSHA256.Create(), _settings.Secret, rawData);
			SetHeaders(new Dictionary<string, string> { { "amx", $"{_settings.ApiKey}:{signature}:{nonce}" } });
			var content = await CallAsync<dynamic>(HttpMethod.Post, uri, new StringContent(String.Empty));

			return CallAsync<dynamic>(HttpMethod.Post, BuildUrl(_settings.PrivateUrl, url));
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