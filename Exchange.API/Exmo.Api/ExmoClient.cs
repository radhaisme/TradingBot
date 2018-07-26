using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace Exmo.Api
{
	public class ExmoClient : ApiClient, IApiClient
	{
		private readonly IExmoSettings _settings;

		public ExmoClient()
		{
			_settings = new ExmoSettings();
		}

		public ExchangeType Type => _settings.Type;

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await CallAsync<Dictionary<string, dynamic>>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "pair_settings"));
			var pairs = new List<PairDto>();

			foreach (string key in content.Keys)
			{
				var dto = new PairDto();
				string[] assets = key.Split('_');
				dto.BaseAsset = assets[0];
				dto.QuoteAsset = assets[1];
				pairs.Add(dto);
			}

			return pairs.AsReadOnly();
		}

		public async Task<PairDetailDto> GetPairDetailAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<Dictionary<string, dynamic>>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "ticker"));
			var dto = new PairDetailDto();
			dynamic item = content[pair];
			dto.LastPrice = item.last_trade;
			dto.High = item.high;
			dto.Low = item.low;
			dto.Volume = item.vol;
			dto.Ask = item.buy_price;
			dto.Bid = item.sell_price;

			return dto;
		}

		public async Task<DepthDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"order_book?pair={pair}&limit={limit}"));
			var dto = Helper.BuildOrderBook(((IEnumerable<dynamic>)content[pair].ask).Take((int)limit), ((IEnumerable<dynamic>)content[pair].bid).Take((int)limit), item => new BookOrderDto { Price = item[0], Amount = item[2] });

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
			throw new HttpRequestException(content.Error);
		}
	}
}