using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Api;

namespace TradingBot.CurrencyProvider
{
	public sealed class CoinMarketCapClient : ApiClient, ICurrencyProvider
	{
		//private readonly ICoinMarketCapSettings _settings;

		//public CoinMarketCapClient()
		//{
		//	_settings = new CoinMarketCapSettings();
		//}

		//public async Task<IReadOnlyDictionary<string, IList<Currency>>> GetCurrenciesAsync()
		//{
		//	var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "listings"));
		//	var currencies = new Dictionary<string, IList<Currency>>();

		//	foreach (dynamic item in content.data)
		//	{
		//		var currency = new Currency((int)item.id, (string)item.symbol, (string)item.name);

		//		if (!currencies.ContainsKey((string)item.symbol))
		//		{
		//			currencies.Add((string)item.symbol, new List<Currency>());
		//		}

		//		currencies[(string)item.symbol].Add(currency);
		//	}

		//	return new ReadOnlyDictionary<string, IList<Currency>>(currencies);
		//}
	}
}