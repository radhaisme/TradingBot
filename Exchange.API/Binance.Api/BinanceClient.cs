using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		public async Task<IReadOnlyCollection<Pair>> GetPairsAsync()
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

			return new ReadOnlyCollection<Pair>(pairs);
		}

		public async Task<PairDetail> GetPairDetailAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairDetail(pair));
			var detail = new PairDetail();
			detail.LastPrice = content.price;

			return detail;
		}
	}
}