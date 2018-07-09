using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Okex.Api
{
	public sealed class OkexClient : IExchangeClient
	{
		private readonly OkexApi _api;
		private readonly IOkexSettings _settings;

		public OkexClient()
		{
			_settings = new OkexSettings();
			_api = new OkexApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IReadOnlyCollection<Pair>> GetPairsAsync()
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairs());
			var pairs = new List<Pair>();

			foreach (dynamic item in content.tickers)
			{
				var pair = new Pair();
				string[] assets = ((string)item.symbol).Split('_');
				pair.BaseAsset = assets[0];
				pair.QuoteAsset = assets[1];
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