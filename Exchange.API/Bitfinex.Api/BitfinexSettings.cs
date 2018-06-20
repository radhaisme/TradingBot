using System;
using TradingBot.Core;

namespace Bitfinex.Api
{
	internal class BitfinexSettings : ApiSettings, IBitfinexSettings
	{
		public BitfinexSettings() : base(typeof(IBitfinexSettings))
		{ }

		public string PublicUrl { get; set; }
		public string PrivateUrl { get; set; }
		public string ApiKey { get; set; }
		public string Secret { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}