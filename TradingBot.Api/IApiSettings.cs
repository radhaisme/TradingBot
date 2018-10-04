using System;

namespace TradingBot.Api
{
	public interface IApiSettings
	{
		string PublicUrl { get; }
		string PrivateUrl { get; set; }
		string ApiKey { get; set; }
		string Secret { get; set; }
		DateTimeOffset CreatedAt { get; }
		uint RequestLimit { get; }
	}
}