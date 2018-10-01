
namespace Bitmex.Api.Models
{
    public sealed class CreateOrderResponse
    {
	    public CreateOrderResponse(long orderId)
	    {
		    OrderId = orderId;
	    }

		public long OrderId { get; }
    }
}