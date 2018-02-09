
namespace TradingBot.Common
{
	using System;

	public class PairData
    {
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Avg { get; set; }
		public decimal Vol { get; set; }
		public decimal VolCur { get; set; }
		public decimal Last { get; set; }
		public decimal Buy { get; set; }
		public decimal Sell { get; set; }
		public DateTimeOffset Updated { get; set; }
    }
}