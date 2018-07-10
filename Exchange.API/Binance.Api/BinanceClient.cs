using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Binance.Api
{
	public sealed class BinanceClient : IExchangeClient
	{
		private readonly BinanceApi _api;
		private readonly IBinanceSettings _settings;

		public BinanceClient()
		{
			_settings = new BinanceSettings();
			_api = new BinanceApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			HttpResponseMessage response = await _api.GetPairsAsync();
			var content = await HttpHelper.AcquireContentAsync<dynamic>(response);
			var pairs = new List<PairDto>();

			foreach (dynamic item in content.symbols)
			{
				var pair = new PairDto();
				pair.BaseAsset = item.baseAsset;
				pair.QuoteAsset = item.quoteAsset;
				//pair.Precision = item.baseAssetPrecision;
				//pair.MinOrderSize = item.filters[0].minPrice;
				//pair.MaxOrderSize = item.filters[0].maxPrice;
				pairs.Add(pair);
			}

			return new ReadOnlyCollection<PairDto>(pairs);
		}

		public async Task<PairDetailDto> GetPairDetailAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairDetailAsync(pair));
			var detail = new PairDetailDto();
			detail.LastPrice = content.price;

			return detail;
		}

		public async Task<OrderBookDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetOrderBookAsync(pair, limit));
			var model = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.asks).Take((int)limit), ((IEnumerable<dynamic>)content.bids).Take((int)limit), item => new OrderDto { Price = item[0], Amount = item[1] });

			return model;
		}
	}
}