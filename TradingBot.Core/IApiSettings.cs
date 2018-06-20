using System;

namespace TradingBot.Core
{
	public interface IApiSettings
	{
		string PublicUrl { get; set; }
		string PrivateUrl { get; set; }
		string ApiKey { get; set; }
		string Secret { get; set; }
		DateTimeOffset CreatedAt { get; set; }
	}
}