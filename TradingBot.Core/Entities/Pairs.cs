
namespace TradingBot.Core.Entities
{
	using System.Collections.Generic;

	public class PairsInfo
	{
		public PairsInfo()
		{
			Pairs = new Dictionary<string, TradePair>();
		}

		public IDictionary<string, TradePair> Pairs { get; set; }
	}
}