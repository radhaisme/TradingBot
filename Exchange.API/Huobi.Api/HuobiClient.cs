using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Huobi.Api
{
	public sealed class HuobiClient : IExchangeClient
	{
		private readonly HuobiApi _api;
		private readonly IHuobiSettings _settings;

		public HuobiClient()
		{
			_settings = new HuobiSettings();
			_api = new HuobiApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IReadOnlyCollection<Pair>> GetPairsAsync()
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairs());
			var pairs = new List<Pair>();

			foreach (dynamic item in content.data)
			{
				var pair = new Pair((string)item["base-currency"] + (string)item["quote-currency"]);
				pair.BaseAsset = ((string)item["base-currency"]).ToUpper();
				pair.QuoteAsset = ((string)item["quote-currency"]).ToUpper();
				//pair.Precision = item["price-precision"];
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
			detail.LastPrice = content.tick.close;
			detail.Ask = content.tick.ask[0];
			detail.Bid = content.tick.bid[0];
			detail.Volume = content.tick.vol;
			detail.High = content.tick.high;
			detail.Low = content.tick.low;

			return detail;
		}

		public async Task<IReadOnlyCollection<PairDetail>> GetPairsDetails(params string[] pairs)
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairsDetails());
			var details = new List<PairDetail>();

			foreach (dynamic item in content.data)
			{
				var detail = new PairDetail();
				detail.LastPrice = item.close;
				//detail.Low = item.low;
				//detail.High = item.high;
				//detail.Volume = item.vol;
				details.Add(detail);
			}

			return new ReadOnlyCollection<PairDetail>(details);
		}
	}
}