
namespace TradingBot.Core.Entities
{
	using System.Collections.Generic;

	public class PairsInfo
	{
		public PairsInfo()
		{
			Pairs = new Dictionary<string, Pair>();
		}

		public IDictionary<string, Pair> Pairs { get; set; }
	}
}