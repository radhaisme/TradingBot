
namespace TradingBot.Core
{
	using Enums;

	public interface IClientFactory
	{
		IExchangeClient Create(ExchangeType type, IApiSettings settings = null);
	}
}