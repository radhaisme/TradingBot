using System.Collections.Generic;

namespace Bitmex.Api.Models
{
	public sealed class DepthResponse
    {
	    public DepthResponse(List<OrderInBookResult> asks, List<OrderInBookResult> bids)
	    {
			Asks = asks.AsReadOnly();
		    Bids = bids.AsReadOnly();
	    }

		public DepthResponse() : this(new List<OrderInBookResult>(0), new List<OrderInBookResult>(0))
		{ }

		public IReadOnlyCollection<OrderInBookResult> Asks { get; }
		public IReadOnlyCollection<OrderInBookResult> Bids { get; }
	}
}