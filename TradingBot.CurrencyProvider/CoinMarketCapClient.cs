using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace TradingBot.CurrencyProvider
{
	public class CoinMarketCapClient : ICurrencyProvider
	{
		private readonly CoinMarketCapApi _api;
		private readonly ICoinMarketCapSettings _settings;

		public CoinMarketCapClient()
		{
			_settings = new CoinMarketCapSettings();
			_api = new CoinMarketCapApi(_settings.PublicUrl, _settings.PublicUrl);
		}

		public async Task<IReadOnlyDictionary<string, IList<Currency>>> GetCurrenciesAsync()
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetCurrenciesAsync());
			var currencies = new Dictionary<string, IList<Currency>>();

			foreach (dynamic item in content.data)
			{
				var currency = new Currency((int)item.id, (string)item.symbol, (string)item.name);

				if (!currencies.ContainsKey((string)item.symbol))
				{
					currencies.Add((string)item.symbol, new List<Currency>());
				}

				currencies[(string)item.symbol].Add(currency);
			}

			return new ReadOnlyDictionary<string, IList<Currency>>(currencies);
		}
	}
}