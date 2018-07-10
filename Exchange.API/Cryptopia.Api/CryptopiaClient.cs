using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairsAsync());
			var pairs = new List<PairDto>();

			foreach (dynamic item in content.Data)
			{
				var pair = new PairDto();
				pair.BaseAssetName = item.Currency;
				pair.BaseAsset = item.Symbol;
				pair.QuoteAssetName = item.BaseCurrency;
				pair.QuoteAsset = item.BaseSymbol;
				pair.MaxOrderSize = item.MaximumTrade;
				pair.MinOrderSize = item.MinimumTrade;
				pairs.Add(pair);
			}

			return new ReadOnlyCollection<PairDto>(pairs);
		}

		public async Task<PairDetailDto> GetPairDetailAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairDetailAsync(pair));
			var detail = new PairDetailDto();
			detail.Ask = content.Data.AskPrice;
			detail.Bid = content.Data.BidPrice;
			detail.High = content.Data.High;
			detail.Low = content.Data.Low;
			detail.LastPrice = content.Data.LastPrice;
			detail.Volume = content.Data.Volume;

			return detail;
		}

		public async Task<OrderBookDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetOrderBookAsync(pair, limit));
			var model = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.Data.Buy).Take((int)limit), ((IEnumerable<dynamic>)content.Data.Sell).Take((int)limit), item => new OrderDto { Price = item.Price, Amount = item.Volume });

			return model;
		}
	}
}