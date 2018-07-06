
namespace TradingBot.Core
{
	public interface ISymbolFormatter
	{
		string Format(string baseAsset, string quoteAsset);
	}
}