using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core.Entities;

namespace Bitfinex.Api
{
	public class BitfinexClient
	{
		private readonly BitfinexApi _api;
		private readonly IBitfinexSettings _settings;

		public BitfinexClient()
		{
			_settings = new BitfinexSettings();
			_api = new BitfinexApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IEnumerable<Pair>> GetPairs()
		{
			HttpResponseMessage[] results = await Task.WhenAll(_api.GetPairs(), _api.GetPairsDetails());
			var items = await HttpHelper.AcquireContentAsync<string[]>(results[0]);
			var details = await HttpHelper.AcquireContentAsync<dynamic>(results[1]);
			var pairs = new Dictionary<string, Pair>(items.Length);

			foreach (var item in items)
			{
				var pair = new Pair(item);
				pairs.Add(item, pair);
			}

			foreach (dynamic detail in details)
			{
				if (!pairs.ContainsKey((string)detail.pair))
				{
					continue;
				}

				Pair pair = pairs[(string)detail.pair];
				pair.Precision = detail.price_precision;
				pair.MaxOrderSize = detail.maximum_order_size;
				pair.MinOrderSize = detail.minimum_order_size;
			}

			return pairs.Values;
		}

		public async Task<PairDetail> GetPairDetail(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairDetail(pair));
			var detail = new PairDetail();
			detail.LastPrice = content.last_price;
			detail.Ask = content.ask;
			detail.Bid = content.bid;
			detail.Avg = content.mid;
			detail.Volume = content.volume;
			detail.High = content.high;
			detail.Low = content.low;

			return detail;
		}
	}
}