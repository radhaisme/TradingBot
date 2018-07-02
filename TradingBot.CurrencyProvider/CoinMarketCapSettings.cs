using TradingBot.Core;

namespace TradingBot.CurrencyProvider
{
	public class CoinMarketCapSettings : ApiSettings, ICoinMarketCapSettings
	{
		private readonly ICoinMarketCapSettings _settings;

		public CoinMarketCapSettings() : base(typeof(ICoinMarketCapSettings))
		{ }
	}
}