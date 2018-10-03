using Newtonsoft.Json;

namespace Bitfinex.Api.Models
{
	public sealed class CancelOrderRequest : OrderRequest
	{
	    public CancelOrderRequest(long orderId)
	    {
		    OrderId = orderId;
	    }

		[JsonProperty(PropertyName = "order_id")]
	    public long OrderId { get; }
	}
}