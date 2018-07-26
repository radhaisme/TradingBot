
namespace TradingBot.Core
{
	using Enums;

	public interface IClientFactory
	{
		IApiClient Create(ExchangeType type, IApiSettings settings = null);
	}
}