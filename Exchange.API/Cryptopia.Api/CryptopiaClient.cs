using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core.Entities;

namespace Cryptopia.Api
{
	public class CryptopiaClient
	{
		private readonly CryptopiaApi _api;
		private readonly ICryptopiaSettings _settings;

		public CryptopiaClient()
		{
			_settings = new CryptopiaSettings();
			_api = new CryptopiaApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IEnumerable<Pair>> GetPairs()
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetTradePairs());
			var pairs = new List<Pair>();

			foreach (dynamic item in content.Data)
			{
				var pair = new Pair($"{item.Symbol}_{item.BaseSymbol}");
				pair.BaseAsset = item.BaseSymbol;
				pair.QuoteAsset = item.Symbol;
				pair.MaxOrderSize = item.MaximumTrade;
				pair.MinOrderSize = item.MinimumTrade;
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
			detail.Ask = content.Data.AskPrice;
			detail.Bid = content.Data.BidPrice;
			detail.High = content.Data.High;
			detail.Low = content.Data.Low;
			detail.Avg = (detail.High + detail.Low) / 2;
			detail.LastPrice = content.Data.LastPrice;
			detail.Volume = content.Data.Volume;

			return detail;
		}
	}
}