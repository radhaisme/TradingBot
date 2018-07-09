
namespace TradingBot.Core.Entities
{
	using System.Collections.Generic;

	public class PairsInfo
	{
		public PairsInfo()
		{
			Pairs = new Dictionary<string, PairDto>();
		}

		public IDictionary<string, PairDto> Pairs { get; set; }
	}
}