using System.Collections.Generic;

namespace TradingBot.Core.Entities
{
	public class PairResponse
	{
		public PairResponse(List<TradePair> pairs)
		{
			Pairs = pairs.AsReadOnly();
		}

		public IReadOnlyCollection<TradePair> Pairs { get; }
	}
}