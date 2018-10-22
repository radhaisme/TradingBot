using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TradingBot.Core;
using TradingBot.Core.Models;

namespace TradingBot.Scanner
{
	public class ExchangePair
	{
		public ExchangePair(IExchange first, IExchange second)
		{
			First = first;
			Second = second;
			Pairs = new ReadOnlyCollection<TradePair>(first.Pairs.Intersect(second.Pairs).ToList());
		}

		public IReadOnlyCollection<TradePair> Pairs { get; }
		public IExchange First { get; }
		public IExchange Second { get; }
	}
}