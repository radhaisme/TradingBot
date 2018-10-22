using System.Collections.Generic;

namespace TradingBot.Core.Models
{
	public class TradePairsResponse
	{
		public TradePairsResponse(List<TradePair> pairs)
		{
			Pairs = pairs.AsReadOnly();
		}

		public IReadOnlyCollection<TradePair> Pairs { get; }
	}
}