using TradingBot.Api;

namespace Exmo.Api
{
	internal class ExmoSettings : ApiSettings, IExmoSettings
	{
		public ExmoSettings() : base(typeof(IExmoSettings))
		{ }
	}
}