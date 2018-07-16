using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Kucoin.Api
{
	public sealed class KucoinClient : ApiClient, IExchangeClient
	{
		private readonly IKucoinSettings _settings;

		public KucoinClient()
		{
			_settings = new KucoinSettings();
		}

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await CallAsync<ResponseModel>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "market/open/symbols"));
			var pairs = new List<PairDto>();

			foreach (dynamic item in content.Data)
			{
				if (!(bool)item.trading)
				{
					continue;
				}

				var pair = new PairDto();
				pair.BaseAsset = item.coinType;
				pair.QuoteAsset = item.coinTypePair;
				pairs.Add(pair);
			}

			return pairs.AsReadOnly();
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
			{
				return;
			}

			var content = await HttpHelper.AcquireContentAsync<ResponseModel>(response);
			string message = content.Msg;

			if (String.IsNullOrEmpty(content.Msg))
			{
				message = "Some error occurred.";
			}

			throw new HttpRequestException(message);
		}

		public async Task<PairDetailDto> GetPairDetailAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<ResponseModel>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"{pair}/open/tick"));

			if (!(bool)content.Data.trading)
			{
				return null;
			}

			dynamic data = content.Data;
			var dto = new PairDetailDto();
			dto.LastPrice = data.lastDealPrice;
			dto.Ask = data.buy;
			dto.Bid = data.sell;
			dto.Volume = data.vol;
			dto.Low = ((decimal?)data.low).GetValueOrDefault();
			dto.High = ((decimal?)data.high).GetValueOrDefault();

			return dto;
		}

		public async Task<OrderBookDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<ResponseModel>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"open/orders?symbol={pair}&limit={limit}"));
			dynamic data = content.Data;
			var dto = Helper.BuildOrderBook(((IEnumerable<dynamic>)data.BUY).Take((int)limit), ((IEnumerable<dynamic>)data.SELL).Take((int)limit), item => new OrderDto { Price = item[0], Amount = item[1] });

			return dto;
		}
	}
}