
namespace TradingBot.Common
{
    public class Pair
    {
		public byte DecimalPlaces { get; set; }
		public decimal MinPrice { get; set; }
		public decimal MaxPrice { get; set; }
		public decimal MinAmount { get; set; }
		public decimal MinTotal { get; set; }
		public bool IsHidden { get; set; }
		public decimal Fee { get; set; }
		public decimal FeeBuyer { get; set; }
		public decimal FeeSeller { get; set; }
    }
}