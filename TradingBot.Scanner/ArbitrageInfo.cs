
namespace TradingBot.Scanner
{
	public class ArbitrageInfo
	{
		public string Symbol { get; set; }
		public string Route { get; set; }
		public decimal BuyPrice { get; set; }
		public decimal SellPrice { get; set; }
		public float Rate { get; set; }
		public bool CanTransfer { get; set; }

		public override string ToString()
		{
			return $"{Symbol},{Route},{BuyPrice},{SellPrice},{Rate:0.##} %";
		}
	}
}