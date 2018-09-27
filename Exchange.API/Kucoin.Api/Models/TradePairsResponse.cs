using System.Collections.Generic;

namespace Kucoin.Api.Models
{
	public sealed class TradePairsResponse
    {
	    public TradePairsResponse(List<TradePairResult> pairs)
	    {
		    Pairs = pairs.AsReadOnly();
	    }

	    public IReadOnlyCollection<TradePairResult> Pairs { get; }
	}
}