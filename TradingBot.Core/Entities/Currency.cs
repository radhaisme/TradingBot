
namespace TradingBot.Core.Entities
{
    public class Currency
    {
	    public Currency(int id, string symbol)
	    {
		    Id = id;
			Symbol = symbol;
	    }

		public int Id { get; }
		public string Symbol { get; }
	}
}