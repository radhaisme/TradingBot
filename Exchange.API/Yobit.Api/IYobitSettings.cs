
namespace Yobit.Api
{
	using TradingBot.Core;

	public interface IYobitSettings : IApiSettings
	{
		int Counter { get; set; }
	}
}