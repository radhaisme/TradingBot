
namespace Cryptopia.Api.Models
{
    public sealed class CancelOrderRequest
    {
	    public CancelOrderRequest(long orderId)
	    {
		    OrderId = orderId;
		    Type = CancelTradeType.Trade;
	    }

	    public CancelOrderRequest(int tradePairId, CancelTradeType type = CancelTradeType.TradePair)
	    {
		    TradePairId = tradePairId;
		    Type = CancelTradeType.TradePair;
	    }

	    public int? TradePairId { get; }
		public long OrderId { get; }
		public CancelTradeType Type { get; }
	}
}