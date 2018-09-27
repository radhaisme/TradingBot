
namespace Bitfinex.Api.Models
{
    public sealed class MarketResponse
    {
	    public int TradePairId { get; set; }
	    public string Label { get; set; }
	    public decimal AskPrice { get; set; }
	    public decimal BidPrice { get; set; }
	    public decimal Low { get; set; }
	    public decimal High { get; set; }
	    public decimal Volume { get; set; }
	    public decimal LastPrice { get; set; }
	    public decimal LastVolume { get; set; }
	    public decimal BuyVolume { get; set; }
	    public decimal SellVolume { get; set; }
	    public decimal Change { get; set; }
	}
}