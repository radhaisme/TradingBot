
namespace TradingBot.Core.Entities
{
	public class Pair
	{
		public Pair(string symbol)
		{
			Symbol = symbol;
		}

		public string Symbol { get; }
		public string BaseAsset { get; set; }
		public string QuoteAsset { get; set; }
		public byte Precision { get; set; }
		public decimal MaxOrderSize { get; set; }
		public decimal MinOrderSize { get; set; }
	}
}