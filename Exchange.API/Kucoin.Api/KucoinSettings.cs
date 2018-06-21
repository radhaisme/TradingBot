using TradingBot.Core;

namespace Kucoin.Api
{
	internal class KucoinSettings : ApiSettings, IKucoinSettings
	{
		public KucoinSettings() : base(typeof(IKucoinSettings))
		{ }
	}
}