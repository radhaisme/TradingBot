
namespace TradingBot.Core.Entities
{
	public class PairDto
	{
		public string BaseAssetName { get; set; }
		public string BaseAsset { get; set; }
		public string QuoteAssetName { get; set; }
		public string QuoteAsset { get; set; }
		public byte Precision { get; set; }
		public decimal MaxOrderSize { get; set; }
		public decimal MinOrderSize { get; set; }
	}
}