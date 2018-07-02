
namespace TradingBot.Core.Entities
{
    public class Currency
    {
	    public Currency(int id, string symbol, string name)
	    {
		    Id = id;
		    Name = name;
			Symbol = symbol;
	    }

		public int Id { get; }
	    public string Name { get; }
		public string Symbol { get; }
	}
}