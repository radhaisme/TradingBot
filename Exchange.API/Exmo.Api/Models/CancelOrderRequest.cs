
namespace Exmo.Api.Models
{
    public sealed class CancelOrderRequest
    {
	    public CancelOrderRequest(long orderId, string pair)
	    {
		    OrderId = orderId;
		    Type = CancelTradeType.Trade;
	    }

		public string Pair { get; set; }
		public long OrderId { get; }
		public CancelTradeType Type { get; }
	}
}