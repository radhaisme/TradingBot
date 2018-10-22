using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Core.Enums;
using TradingBot.Core.Models;

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
		//public IReadOnlyCollection<TradePair> Pairs { get; private set; }

		//void IExchange.Initialize()
		//{
		//	var pairs = new List<TradePair>();

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

		//		var tradePair = new TradePair(bases[0], quotes[0]);
		//		pairs.Add(tradePair);
		//	}

		//	Pairs = pairs.AsReadOnly();
		//}

		//public async Task<decimal> GetPriceAsync(TradePair tradePair)
		//{
		//	PairDetailResponse detailResponse = null;//await _client.GetPairDetailAsync(tradePair.GetSymbol(Type));

		//	return detailResponse.LastPrice;
		//}

		//public async Task<(decimal ask, decimal bid)> GetBookOrderPriceAsync(TradePair tradePair)
		//{
		//	//DepthResponse response = await _client.GetOrderBookAsync(tradePair.GetSymbol(Type), 5);
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
		public IReadOnlyCollection<TradePair> Pairs { get; }
		public void Initialize()
		{
			throw new NotImplementedException();
		}

		public Task<decimal> GetPriceAsync(TradePair tradePair)
		{
			throw new NotImplementedException();
		}

		public Task<(decimal ask, decimal bid)> GetBookOrderPriceAsync(TradePair tradePair)
		{
			throw new NotImplementedException();
		}
	}
}