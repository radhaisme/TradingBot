
namespace Yobit.Api.Entities
{
    public enum OrderStatus : byte
    {
		Active = 0,
	    Fulfilled,
		Closed = Fulfilled,
	    Cancelled,
		CancelledPartiallyFulfilled //Ugly name. Need to think about to change name with the more shortes
	}
}