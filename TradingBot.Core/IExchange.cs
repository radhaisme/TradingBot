using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Core.Enums;
using TradingBot.Core.Models;

namespace TradingBot.Core
{
	public interface IExchange
	{
		ExchangeType Type { get; }
		IReadOnlyCollection<TradePair> Pairs { get; }
		void Initialize();
		Task<decimal> GetPriceAsync(TradePair tradePair);
		Task<(decimal ask, decimal bid)> GetBookOrderPriceAsync(TradePair tradePair);
	}
}