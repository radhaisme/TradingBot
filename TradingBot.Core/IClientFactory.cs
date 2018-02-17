
namespace TradingBot.Core
{
	using Enums;

	public interface IClientFactory
	{
		IExchangeClient Create(Exchange type, IApiSettings settings = null);
	}
}