
namespace TradingBot.Core
{
	public interface IExchangeClient
	{
		//object GetPairs(); //For test purpose

		dynamic GetActiveOrdersOfUser(string pair);
	}
}