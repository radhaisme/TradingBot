using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Core.Entities;

namespace TradingBot.Core
{
	public interface ICurrencyProvider
	{
		Task<IEnumerable<Currency>> GetCurrenciesAsync();
	}
}