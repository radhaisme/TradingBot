using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		private readonly KucoinApi _api;
		private readonly IKucoinSettings _settings;

		public KucoinClient()
		{
			_settings = new KucoinSettings();
			_api = new KucoinApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			try
			{
				var content = await CallAsync<ResponseModel>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "market/open/symbols2"));
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

				return new ReadOnlyCollection<PairDto>(pairs);
			}
			catch (HttpRequestException ex)
			{
				throw;
			}
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			var content = (ResponseModel)await HttpHelper.AcquireContentAsync(response, typeof(ResponseModel));
			throw new HttpRequestException(content.Msg);
		}

		public async Task<PairDetailDto> GetPairDetailAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairDetailAsync(pair));

			if (!(bool)content.data.trading)
			{
				return null;
			}

			var detail = new PairDetailDto();
			detail.LastPrice = content.data.lastDealPrice;
			detail.Ask = content.data.buy;
			detail.Bid = content.data.sell;
			detail.Volume = content.data.vol;
			detail.Low = ((decimal?)content.data.low).GetValueOrDefault();
			detail.High = ((decimal?)content.data.high).GetValueOrDefault();

			return detail;
		}

		public async Task<OrderBookDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<ResponseModel>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"open/orders?symbol={pair}&limit={limit}"));
			var model = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.Data.BUY).Take((int)limit), ((IEnumerable<dynamic>)content.Data.SELL).Take((int)limit), item => new OrderDto { Price = item[0], Amount = item[1] });

			return model;
		}
	}
}