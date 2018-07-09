
namespace TradingBot.Core.Entities
{
	public class PairDetailDto
	{
		public decimal High { get; set; }
		public decimal Low { get; set; }

		public decimal Avg
		{
			get
			{
				if (Ask > 0 && Bid > 0)
				{
					return (Bid + Ask) / 2;
				}

				return 0;
			}
		}

		public decimal Volume { get; set; }
		public decimal LastPrice { get; set; }
		public decimal Ask { get; set; }
		public decimal Bid { get; set; }
	}
}