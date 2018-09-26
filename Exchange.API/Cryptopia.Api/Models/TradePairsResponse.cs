using System.Collections.Generic;
using TradingBot.Core.Entities;

namespace Cryptopia.Api.Models
{
	public sealed class TradePairsResponse
    {
	    public TradePairsResponse(List<TradePair> pairs)
	    {
		    Pairs = pairs.AsReadOnly();
	    }

	    public IReadOnlyCollection<TradePair> Pairs { get; }
	}
}