using System;
using TradingBot.Core;

namespace Binance.Api
{
	internal class BinanceSettings : ApiSettings, IBinanceSettings
	{
		public BinanceSettings() : base(typeof(IBinanceSettings))
		{ }

		public string PublicUrl { get; set; }
		public string PrivateUrl { get; set; }
		public string ApiKey { get; set; }
		public string Secret { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}