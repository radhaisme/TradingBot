using System.Collections.Generic;

namespace TradingBot.Core.Entities
{
	public class PairResponse
	{
		public PairResponse(List<PairDto> pairs)
		{
			Pairs = pairs.AsReadOnly();
		}

		public IReadOnlyCollection<PairDto> Pairs { get; }
	}
}