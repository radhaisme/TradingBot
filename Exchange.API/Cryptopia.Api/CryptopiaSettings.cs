using TradingBot.Core;

namespace Cryptopia.Api
{
	internal class CryptopiaSettings : ApiSettings, ICryptopiaSettings
	{
		public CryptopiaSettings() : base(typeof(ICryptopiaSettings))
		{ }
	}
}