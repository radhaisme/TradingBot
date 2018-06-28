using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core.Entities;

namespace Huobi.Api
{
	public class HuobiClient
	{
		private readonly HuobiApi _api;
		private readonly IHuobiSettings _settings;

		public HuobiClient()
		{
			_settings = new HuobiSettings();
			_api = new HuobiApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IEnumerable<Pair>> GetPairs()
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairs());
			var pairs = new List<Pair>();

			foreach (dynamic item in content.data)
			{
				var pair = new Pair((string)item["base-currency"] + (string)item["quote-currency"]);
				pair.BaseAsset = item["base-currency"];
				pair.QuoteAsset = item["quote-currency"];
				pair.Precision = item["price-precision"];
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
			detail.LastPrice = content.tick.close;
			detail.Ask = content.tick.ask[0];
			detail.Bid = content.tick.bid[0];
			detail.Volume = content.tick.vol;
			detail.High = content.tick.high;
			detail.Low = content.tick.low;

			return detail;
		}
	}
}