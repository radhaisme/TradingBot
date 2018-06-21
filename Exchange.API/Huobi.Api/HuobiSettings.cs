using TradingBot.Core;

namespace Huobi.Api
{
	internal class HuobiSettings : ApiSettings, IHuobiSettings
	{
		public HuobiSettings() : base(typeof(IHuobiSettings))
		{ }
	}
}