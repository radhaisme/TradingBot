
namespace TradingBot.Core.Models
{
    public class Currency
    {
        public Currency(int id, string symbol, string name)
        {
            Id = id;
            Symbol = symbol;
            Name = name;
        }

        public int Id { get; }
        public string Symbol { get; }
        public string Name { get; }
    }
}