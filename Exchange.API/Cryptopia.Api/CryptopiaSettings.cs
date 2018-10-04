using TradingBot.Api;

namespace Cryptopia.Api
{
	internal class CryptopiaSettings : ApiSettings, ICryptopiaSettings
	{
		public CryptopiaSettings() : base(typeof(ICryptopiaSettings))
		{ }
	}
}