using System;
using TradingBot.Core;

namespace Exmo.Api
{
	internal class ExmoSettings : ApiSettings, IExmoSettings
	{
		public ExmoSettings() : base(typeof(IExmoSettings))
		{ }

		public string PublicUrl { get; set; }
		public string PrivateUrl { get; set; }
		public string ApiKey { get; set; }
		public string Secret { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}