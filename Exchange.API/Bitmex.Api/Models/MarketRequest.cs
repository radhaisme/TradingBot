
namespace Bitmex.Api.Models
{
    public sealed class MarketRequest
    {
	    public MarketRequest(string pair)
	    {
		    Pair = pair;
	    }

	    public string Pair { get; }
	}
}