
namespace Yobit.Api
{
	using System;
	using TradingBot.Core;

	public interface IYobitSettings : IApiSettings
	{
		DateTimeOffset CreatedAt { get; set; }
	}
}