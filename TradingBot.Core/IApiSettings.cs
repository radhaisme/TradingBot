using System;
using TradingBot.Core.Enums;

namespace TradingBot.Core
{
	public interface IApiSettings
	{
		ExchangeType Type { get; set; }
		string PublicUrl { get; }
		string PrivateUrl { get; set; }
		string ApiKey { get; set; }
		string Secret { get; set; }
		DateTimeOffset CreatedAt { get; }
		uint RequestLimit { get; }
	}
}