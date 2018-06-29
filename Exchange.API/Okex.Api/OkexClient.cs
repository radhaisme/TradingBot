using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core.Entities;

namespace Okex.Api
{
	public sealed class OkexClient
	{
		private readonly OkexApi _api;
		private readonly IOkexSettings _settings;

		public OkexClient()
		{
			_settings = new OkexSettings();
			_api = new OkexApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IEnumerable<Pair>> GetPairs()
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairs());
			var pairs = new List<Pair>();

			foreach (dynamic item in content.tickers)
			{
				var pair = new Pair((string)item.symbol);
				pairs.Add(pair);
			}

			return pairs;
		}

		public async Task<PairDetail> GetPairDetail(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairDetail(pair));
			var detail = new PairDetail();
			detail.LastPrice = content.ticker.last;
			detail.Ask = content.ticker.buy;
			detail.Bid = content.ticker.sell;
			detail.High = content.ticker.high;
			detail.Low = content.ticker.low;
			detail.Volume = content.ticker.vol;

			return detail;
		}
	}
}