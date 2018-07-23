using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace TradingBot.Core
{
	public sealed class Exchange : IExchange
	{
		private readonly IExchangeClient _client;
		private readonly IReadOnlyDictionary<string, IList<Currency>> _currencies;

		public Exchange(IReadOnlyDictionary<string, IList<Currency>> currencies, IExchangeClient client)
		{
			_currencies = currencies;
			_client = client;
		}

		public ExchangeType Type => _client.Type;
		public IReadOnlyCollection<Pair> Pairs { get; private set; }

		void IExchange.Initialize()
		{
			var pairs = new List<Pair>();

			foreach (PairDto item in _client.GetPairsAsync().Result)
			{
				if (!_currencies.ContainsKey(item.BaseAsset) || !_currencies.ContainsKey(item.QuoteAsset))
				{
					continue;
				}

				IList<Currency> bases = _currencies[item.BaseAsset];
				IList<Currency> quotes = _currencies[item.QuoteAsset];

				if (bases.Count > 1 || quotes.Count > 1)
				{
					continue;
				}

				if (!String.IsNullOrEmpty(item.BaseAssetName) && !String.IsNullOrEmpty(item.QuoteAssetName) && !item.BaseAssetName.Equals(bases[0].Name) && !item.QuoteAssetName.Equals(quotes[0].Name))
				{
					continue;
				}

				var pair = new Pair(bases[0], quotes[0]);
				pairs.Add(pair);
			}

			Pairs = pairs.AsReadOnly();
		}

		public async Task<decimal> GetPriceAsync(Pair pair)
		{
			PairDetailDto detailDto = await _client.GetPairDetailAsync(pair.GetSymbol(Type));

			return detailDto.LastPrice;
		}

		public async Task<(decimal ask, decimal bid)> GetBookOrderPriceAsync(Pair pair)
		{
			OrderBookDto dto = await _client.GetOrderBookAsync(pair.GetSymbol(Type), 5);
			decimal ask = 0;
			decimal bid = 0;

			if (dto.Asks.Count == 0 || dto.Bids.Count == 0)
			{
				return (0, 0);
			}

			foreach (OrderDto item in dto.Asks)
			{
				ask += item.Price;
			}

			foreach (OrderDto item in dto.Bids)
			{
				bid += item.Price;
			}

			return (ask / dto.Asks.Count, bid / dto.Bids.Count);
		}
	}
}