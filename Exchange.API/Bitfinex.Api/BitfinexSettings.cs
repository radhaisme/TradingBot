using TradingBot.Core;

namespace Bitfinex.Api
{
	internal class BitfinexSettings : ApiSettings, IBitfinexSettings
	{
		public BitfinexSettings() : base(typeof(IBitfinexSettings))
		{ }
	}
}