using TradingBot.Api;

namespace Yobit.Api
{
	internal sealed class YobitSettings : ApiSettings, IYobitSettings
	{
		public YobitSettings() : base(typeof(IYobitSettings))
		{ }
	}
}