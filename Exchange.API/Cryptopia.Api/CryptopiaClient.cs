using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Cryptopia.Api
{
	public sealed class CryptopiaClient : IExchangeClient
	{
		private readonly CryptopiaApi _api;
		private readonly ICryptopiaSettings _settings;

		public CryptopiaClient()
		{
			_settings = new CryptopiaSettings();
			_api = new CryptopiaApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IReadOnlyCollection<Pair>> GetPairsAsync()
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairsAsync());
			var pairs = new List<Pair>();

			foreach (dynamic item in content.Data)
			{
				var pair = new Pair();
				pair.BaseAssetName = item.Currency;
				pair.BaseAsset = item.Symbol;
				pair.QuoteAssetName = item.BaseCurrency;
				pair.QuoteAsset = item.BaseSymbol;
				pair.MaxOrderSize = item.MaximumTrade;
				pair.MinOrderSize = item.MinimumTrade;
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
			detail.Ask = content.Data.AskPrice;
			detail.Bid = content.Data.BidPrice;
			detail.High = content.Data.High;
			detail.Low = content.Data.Low;
			detail.LastPrice = content.Data.LastPrice;
			detail.Volume = content.Data.Volume;

			return detail;
		}
	}
}