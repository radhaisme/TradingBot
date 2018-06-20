using System;
using TradingBot.Core;

namespace Kucoin.Api
{
	internal class KucoinSettings : ApiSettings, IKucoinSettings
	{
		public KucoinSettings() : base(typeof(IKucoinSettings))
		{ }

		public string PublicUrl { get; set; }
		public string PrivateUrl { get; set; }
		public string ApiKey { get; set; }
		public string Secret { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}