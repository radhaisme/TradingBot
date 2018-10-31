
namespace TradingBot.Core.Models
{
	public class Currency
	{
		public Currency(int id, string symbol, string name)
		{
			Id = id;
			Symbol = symbol.ToUpper();
			Name = name;
		}

		public Currency(string symbol) : this(0, symbol, null)
		{ }

		public int Id { get; }
		public string Symbol { get; }
		public string Name { get; }
	}
}