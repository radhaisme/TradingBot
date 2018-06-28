using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core.Entities;

namespace Binance.Api
{
	public sealed class BinanceClient
	{
		private readonly BinanceApi _api;
		private readonly IBinanceSettings _settings;

		public BinanceClient()
		{
			_settings = new BinanceSettings();
			_api = new BinanceApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IEnumerable<Pair>> GetPairs()
		{
			HttpResponseMessage response = await _api.GetPairs();
			var content = await HttpHelper.AcquireContentAsync<dynamic>(response);
			var pairs = new List<Pair>();

			foreach (dynamic item in content.symbols)
			{
				var pair = new Pair((string)item.symbol);
				pair.BaseAsset = item.baseAsset;
				pair.QuoteAsset = item.quoteAsset;
				pair.Precision = item.baseAssetPrecision;
				pair.MinOrderSize = item.filters[0].minPrice;
				pair.MaxOrderSize = item.filters[0].maxPrice;
				pairs.Add(pair);
			}

			return pairs;
		}

		public async Task<PairDetail> GetPairDetail(string pair)
		{
			HttpResponseMessage response = await _api.GetPairDetail(pair);
			var content = await HttpHelper.AcquireContentAsync<dynamic>(response);
			var detail = new PairDetail();
			detail.LastPrice = content.price;

			return detail;
		}
	}
}