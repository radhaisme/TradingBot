using TradingBot.Core;

namespace Okex.Api
{
	internal class OkexSettings : ApiSettings, IOkexSettings
	{
		public OkexSettings() : base(typeof(IOkexSettings))
		{ }
	}
}