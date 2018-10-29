
namespace Bitfinex.Api.Models
{
	public class TradePairResult
	{
		public TradePairResult(string baseAsset, string quoteAsset)
		{
			BaseAsset = baseAsset;
			QuoteAsset = quoteAsset;
		}

		public string BaseAssetName { get; set; }
		public string BaseAsset { get; }
		public string QuoteAssetName { get; set; }
		public string QuoteAsset { get; }
		public byte Precision { get; set; }
		public decimal MaxOrderSize { get; set; }
		public decimal MinOrderSize { get; set; }
	}
}