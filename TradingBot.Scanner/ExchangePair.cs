using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace TradingBot.Scanner
{
	public class ExchangePair
	{
		public ExchangePair(IExchange first, IExchange second)
		{
			First = first;
			Second = second;
			Pairs = new ReadOnlyCollection<Pair>(first.Pairs.Intersect(second.Pairs).ToList());
		}

		public IReadOnlyCollection<Pair> Pairs { get; }
		public IExchange First { get; }
		public IExchange Second { get; }
	}
}