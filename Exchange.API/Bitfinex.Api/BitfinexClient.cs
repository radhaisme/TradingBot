using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

		public Task<CreateOrderDto> CreateOrderAsync(OrderDto input)
		{
			throw new NotImplementedException();
		}

		public Task<CancelOrderDto> CancelOrderAsync(CancelOrderDto input)
		{
			throw new NotImplementedException();
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
	}
}