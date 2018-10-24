
namespace TradingBot.Core.Models
{
	public class TradePair //: IEquatable<TradePair>
	{
		private readonly Currency _baseAsset;
		private readonly Currency _quoteAsset;

		public TradePair(Currency baseAsset, Currency quoteAsset)
		{
			_baseAsset = baseAsset;
			_quoteAsset = quoteAsset;
		}

		public string Label => $"{_baseAsset.Symbol}/{_quoteAsset.Symbol}";

		//public string GetSymbol(ExchangeType type)
		//{
		//	switch (type)
		//	{
		//		case ExchangeType.Binance:
		//			{
		//				return (BaseAsset.Symbol + QuoteAsset.Symbol).ToUpper();
		//			}
		//		case ExchangeType.Huobi:
		//		case ExchangeType.Bitfinex:
		//			{
		//				return (BaseAsset.Symbol + QuoteAsset.Symbol).ToLower();
		//			}
		//		case ExchangeType.Exmo:
		//		case ExchangeType.Cryptopia:
		//			{
		//				return $"{BaseAsset.Symbol}_{QuoteAsset.Symbol}".ToUpper();
		//			}
		//		case ExchangeType.Kucoin:
		//			{
		//				return $"{BaseAsset.Symbol}-{QuoteAsset.Symbol}".ToUpper();
		//			}
		//		case ExchangeType.Yobit:
		//		case ExchangeType.Okex:
		//			{
		//				return $"{BaseAsset.Symbol}_{QuoteAsset.Symbol}".ToLower();
		//			}
		//		default:
		//			{
		//				return null;
		//			}
		//	}
		//}

		//public override bool Equals(object obj)
		//{
		//	return Equals((TradePair)obj);
		//}

		//public override int GetHashCode()
		//{
		//	return BaseAsset.Id ^ QuoteAsset.Id;
		//}

		//public bool Equals(TradePair other)
		//{
		//	if (other == null)
		//	{
		//		return false;
		//	}

		//	return BaseAsset.Id == other.BaseAsset.Id && QuoteAsset.Id == other.QuoteAsset.Id;
		//}
	}
}