
namespace TradingBot.Core
{
	using Enums;

	public interface IClientFactory
	{
		IExchangeClient Create(AccountType type, IApiSettings settings = null);
	}
}