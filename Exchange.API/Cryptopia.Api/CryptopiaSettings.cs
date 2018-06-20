using System;
using TradingBot.Core;

namespace Cryptopia.Api
{
	internal class CryptopiaSettings : ApiSettings, ICryptopiaSettings
	{
		public CryptopiaSettings() : base(typeof(ICryptopiaSettings))
		{ }

		public string PublicUrl { get; set; }
		public string PrivateUrl { get; set; }
		public string ApiKey { get; set; }
		public string Secret { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}