
namespace Binance.Api.Models
{
    public sealed class CancelOrderResponse
    {
	    public CancelOrderResponse(long orderId)
	    {
		    OrderId = orderId;
	    }

		public long OrderId { get; set; }
    }
}