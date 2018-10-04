using TradingBot.Api;

namespace Bitfinex.Api
{
	internal class BitfinexSettings : ApiSettings, IBitfinexSettings
	{
		public BitfinexSettings() : base(typeof(IBitfinexSettings))
		{ }
	}
}