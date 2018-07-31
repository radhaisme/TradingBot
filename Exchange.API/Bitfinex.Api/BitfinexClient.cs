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

namespace Bitfinex.Api
{
	public sealed class BitfinexClient : ApiClient, IApiClient
	{
		private readonly IBitfinexSettings _settings;

		public BitfinexClient()
		{
			_settings = new BitfinexSettings();
		}

		public ExchangeType Type => _settings.Type;

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await CallAsync<string[]>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "symbols"));
			var pairs = new Dictionary<string, PairDto>(content.Length);

			foreach (var item in content)
			{
				var dto = new PairDto();
				dto.BaseAsset = item.Substring(0, item.Length - 3);
				dto.QuoteAsset = item.Substring(item.Length - 3, 3);
				pairs.Add(item, dto);
			}

			return pairs.Values.ToList().AsReadOnly();
		}

		public async Task<PairDetailDto> GetPairDetailAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"pubticker/{pair}"));
			var dto = new PairDetailDto();
			dto.LastPrice = content.last_price;
			dto.Ask = content.ask;
			dto.Bid = content.bid;
			dto.Volume = content.volume;
			dto.High = content.high;
			dto.Low = content.low;

			return dto;
		}

		public async Task<DepthDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"book/{pair}?limit_bids={limit}&limit_asks={limit}"));
			var dto = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.asks).Take((int)limit), ((IEnumerable<dynamic>)content.bids).Take((int)limit), item => new BookOrderDto { Price = item.price, Amount = item.amount });

			return dto;
		}

		public async Task<CreateOrderDto> CreateOrderAsync(OrderDto input)
		{
			var order = new { symbol = input.Pair, amount = input.Amount, price = input.Price, side = input.Side.ToString().ToLower(), type = GetOrderType(input.Type), ocoorder = false };
			var content = await MakePrivateCallAsync(order, "order/new");
			var dto = new CreateOrderDto();
			dto.OrderId = content.order_id;

			return dto;
		}

		public async Task<CancelOrderDto> CancelOrderAsync(CancelOrderDto input)
		{
			var order = new { order_id = input.OrderId };
			var content = await MakePrivateCallAsync(order, "order/cancel");
			var dto = new CancelOrderDto();
			dto.OrderId = content.order_id;

			return dto;
		}

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