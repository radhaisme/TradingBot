using TradingBot.Api;

namespace Huobi.Api
{
	internal class HuobiSettings : ApiSettings, IHuobiSettings
	{
		public HuobiSettings() : base(typeof(IHuobiSettings))
		{ }
	}
}