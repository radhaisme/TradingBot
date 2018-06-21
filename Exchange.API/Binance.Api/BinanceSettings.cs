using TradingBot.Core;

namespace Binance.Api
{
	internal class BinanceSettings : ApiSettings, IBinanceSettings
	{
		public BinanceSettings() : base(typeof(IBinanceSettings))
		{ }
	}
}