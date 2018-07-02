using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace TradingBot.CurrencyProvider
{
	public class CoinMarketCapClient : ExchangeApi, ICurrencyProvider
	{
		private readonly CoinMarketCapApi _api;
		private readonly ICoinMarketCapSettings _settings;

		public CoinMarketCapClient() : base("a", "b") //TODO: Refactor this peace of shit
		{
			_settings = new CoinMarketCapSettings();
			_api = new CoinMarketCapApi(_settings.PublicUrl, _settings.PublicUrl);
		}

		public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
		{


			return null;
		}
	}
}