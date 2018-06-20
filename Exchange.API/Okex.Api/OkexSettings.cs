using System;
using TradingBot.Core;

namespace Okex.Api
{
	internal class OkexSettings : ApiSettings, IOkexSettings
	{
		public OkexSettings() : base(typeof(IOkexSettings))
		{ }

		public string PublicUrl { get; set; }
		public string PrivateUrl { get; set; }
		public string ApiKey { get; set; }
		public string Secret { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}