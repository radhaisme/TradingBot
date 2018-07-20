using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace TradingBot.Core
{
	public interface IExchange
	{
		ExchangeType Type { get; }
		IReadOnlyCollection<Pair> Pairs { get; }
		void Initialize();
		Task<decimal> GetPriceAsync(Pair pair);
		Task<(decimal ask, decimal bid)> GetBookOrderPriceAsync(Pair pair);
	}
}