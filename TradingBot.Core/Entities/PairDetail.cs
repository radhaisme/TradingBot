
namespace TradingBot.Core.Entities
{
	public class PairDetail
	{
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Avg => (Bid + Ask) / 2;
		public decimal Volume { get; set; }
		public decimal LastPrice { get; set; }
		public decimal Ask { get; set; }
		public decimal Bid { get; set; }
	}
}