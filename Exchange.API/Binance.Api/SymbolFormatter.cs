using TradingBot.Core;

namespace Binance.Api
{
	public class SymbolFormatter : ISymbolFormatter
	{
		public string Format(string baseAsset, string quoteAsset)
		{
			return (baseAsset + quoteAsset).ToUpper();
		}
	}
}