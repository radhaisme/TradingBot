using TradingBot.Api;

namespace Bitmex.Api
{
    internal class BitmexSettings : ApiSettings, IBitmexSettings
	{
		public BitmexSettings() : base(typeof(IBitmexSettings))
		{ }
	}
}