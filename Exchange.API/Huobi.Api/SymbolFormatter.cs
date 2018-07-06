using TradingBot.Core;

namespace Huobi.Api
{
    public class SymbolFormatter : ISymbolFormatter
	{
		public string Format(string baseAsset, string quoteAsset)
		{
			return (baseAsset + quoteAsset).ToLower();
		}
	}
}