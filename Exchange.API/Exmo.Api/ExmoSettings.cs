using TradingBot.Core;

namespace Exmo.Api
{
	internal class ExmoSettings : ApiSettings, IExmoSettings
	{
		public ExmoSettings() : base(typeof(IExmoSettings))
		{ }
	}
}