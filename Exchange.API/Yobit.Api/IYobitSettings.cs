
namespace Yobit.Api
{
	using System;
	using TradingBot.Core;

	public interface IYobitSettings : IApiSettings
	{
		DateTime CreatedOn { get; set; }
	}
}