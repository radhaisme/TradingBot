using System.Collections.Generic;
using TradingBot.Core.Entities;

namespace Cryptopia.Api.Models
{
	public sealed class DepthResponse
    {
	    public DepthResponse(List<OrderInBook> asks, List<OrderInBook> bids)
	    {
			Asks = asks.AsReadOnly();
		    Bids = bids.AsReadOnly();
	    }

		public DepthResponse() : this(new List<OrderInBook>(0), new List<OrderInBook>(0))
		{ }

		public IReadOnlyCollection<OrderInBook> Asks { get; }
		public IReadOnlyCollection<OrderInBook> Bids { get; }
	}
}