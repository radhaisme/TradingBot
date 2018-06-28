using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core.Entities;

namespace Exmo.Api
{
	public class ExmoClient
	{
		private readonly ExmoApi _api;
		private readonly IExmoSettings _settings;

		public ExmoClient()
		{
			_settings = new ExmoSettings();
			_api = new ExmoApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IEnumerable<Pair>> GetPairs()
		{
			var content = await HttpHelper.AcquireContentAsync<Dictionary<string, dynamic>>(await _api.GetPairs());
			var pairs = new List<Pair>();

			foreach (string key in content.Keys)
			{
				dynamic item = content[key];
				var pair = new Pair(key);
				pair.MinOrderSize = item.min_quantity;
				pair.MaxOrderSize = item.max_quantity;
				pairs.Add(pair);
			}

			return pairs;
		}

		public async Task<IEnumerable<PairDetail>> GetPairsDetails()
		{
			var content = await HttpHelper.AcquireContentAsync<Dictionary<string, dynamic>>(await _api.GetPairsDetails());
			var details = new List<PairDetail>();

			foreach (string key in content.Keys)
			{
				dynamic item = content[key];
				var detail = new PairDetail();
				detail.LastPrice = item.last_trade;
				detail.High = item.high;
				detail.Low = item.low;
				detail.Volume = item.vol;
				detail.Avg = item.avg;
				detail.Ask = item.buy_price;
				detail.Bid = item.sell_price;
				details.Add(detail);
			}

			return details;
		}
	}
}