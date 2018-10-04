using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace TradingBot.Core
{
	public sealed class Exchange : IExchange
	{
		//private readonly IApiClient _client;
		//private readonly IReadOnlyDictionary<string, IList<Currency>> _currencies;

		//public Exchange(IReadOnlyDictionary<string, IList<Currency>> currencies, IApiClient client)
		//{
		//	_currencies = currencies;
		//	_client = client;
		//}

		//public ExchangeType Type => _client.Type;
		//public IReadOnlyCollection<Pair> Pairs { get; private set; }

		//void IExchange.Initialize()
		//{
		//	var pairs = new List<Pair>();

		//	foreach (TradePair item in _client.GetTradePairsAsync().Result.Pairs)
		//	{
		//		if (!_currencies.ContainsKey(item.BaseAsset) || !_currencies.ContainsKey(item.QuoteAsset))
		//		{
		//			continue;
		//		}

		//		IList<Currency> bases = _currencies[item.BaseAsset];
		//		IList<Currency> quotes = _currencies[item.QuoteAsset];

		//		if (bases.Count > 1 || quotes.Count > 1)
		//		{
		//			continue;
		//		}

		//		if (!String.IsNullOrEmpty(item.BaseAssetName) && !String.IsNullOrEmpty(item.QuoteAssetName) && !item.BaseAssetName.Equals(bases[0].Name) && !item.QuoteAssetName.Equals(quotes[0].Name))
		//		{
		//			continue;
		//		}

		//		var pair = new Pair(bases[0], quotes[0]);
		//		pairs.Add(pair);
		//	}

		//	Pairs = pairs.AsReadOnly();
		//}

		//public async Task<decimal> GetPriceAsync(Pair pair)
		//{
		//	PairDetailResponse detailResponse = null;//await _client.GetPairDetailAsync(pair.GetSymbol(Type));

		//	return detailResponse.LastPrice;
		//}

		//public async Task<(decimal ask, decimal bid)> GetBookOrderPriceAsync(Pair pair)
		//{
		//	//DepthResponse response = await _client.GetOrderBookAsync(pair.GetSymbol(Type), 5);
		//	//decimal ask = 0;
		//	//decimal bid = 0;

		//	//if (response.Asks.Count == 0 || response.Bids.Count == 0)
		//	//{
		//	//	return (0, 0);
		//	//}

		//	//foreach (BookOrderDto item in response.Asks)
		//	//{
		//	//	ask += item.Rate;
		//	//}

		//	//foreach (BookOrderDto item in response.Bids)
		//	//{
		//	//	bid += item.Rate;
		//	//}

		//	//return (ask / response.Asks.Count, bid / response.Bids.Count);

		//	return (0, 0);
		//}
		public ExchangeType Type { get; }
		public IReadOnlyCollection<Pair> Pairs { get; }
		public void Initialize()
		{
			throw new NotImplementedException();
		}

		public Task<decimal> GetPriceAsync(Pair pair)
		{
			throw new NotImplementedException();
		}

		public Task<(decimal ask, decimal bid)> GetBookOrderPriceAsync(Pair pair)
		{
			throw new NotImplementedException();
		}
	}
}