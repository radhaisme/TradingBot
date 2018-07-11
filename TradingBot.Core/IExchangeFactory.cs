using TradingBot.Core.Enums;

namespace TradingBot.Core
{
	public interface IExchangeFactory
	{
		IExchange Create(ExchangeType type);
	}
}