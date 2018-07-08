using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Kucoin.Api
{
	public sealed class KucoinClient : IExchangeClient
	{
		private readonly KucoinApi _api;
		private readonly IKucoinSettings _settings;

		public KucoinClient()
		{
			_settings = new KucoinSettings();
			_api = new KucoinApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IReadOnlyCollection<Pair>> GetPairsAsync()
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairs());
			var pairs = new List<Pair>();

			foreach (dynamic item in content.data)
			{
				if (!(bool)item.trading)
				{
					continue;
				}

				var pair = new Pair($"{item.coinType}-{item.coinTypePair}");
				pair.BaseAsset = item.coinType;
				pair.QuoteAsset = item.coinTypePair;
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

			if (!(bool)content.data.trading)
			{
				return null;
			}

			var detail = new PairDetail();
			detail.LastPrice = content.data.lastDealPrice;
			detail.Ask = content.data.buy;
			detail.Bid = content.data.sell;
			detail.Volume = content.data.vol;
			detail.Low = content.data.low;
			detail.High = content.data.high;

			return detail;
		}
	}
}