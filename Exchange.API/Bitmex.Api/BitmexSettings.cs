using TradingBot.Core;

namespace Bitmex.Api
{
    internal class BitmexSettings : ApiSettings, IBitmexSettings
	{
		public BitmexSettings() : base(typeof(IBitmexSettings))
		{ }
	}
}