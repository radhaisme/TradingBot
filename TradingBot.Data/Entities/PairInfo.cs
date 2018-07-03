using System;
using System.Collections.Generic;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace TradingBot.Data.Entities
{
	public class PairInfo : Entity
	{
		public string Name { get; set; }

		public DateTime? UpdatedDt { get; set; }

		public ExchangeType AccountType { get; set; }

		public byte? DecimalPlaces { get; set; }

		public decimal? MinPrice { get; set; }

		public decimal? MaxPrice { get; set; }

		public decimal? MinAmount { get; set; }

		public decimal? MinTotal { get; set; }

		public bool? IsHidden { get; set; }

		public decimal? Fee { get; set; }

		public decimal? FeeBuyer { get; set; }

		public decimal? FeeSeller { get; set; }
	}

	public class Pair : IEquatable<Pair>
	{
		public Pair(Currency baseAsset, Currency quoteAsset)
		{
			BaseAsset = baseAsset;
			QuoteAsset = quoteAsset;
		}

		public string Label => $"{BaseAsset.Symbol}/{QuoteAsset.Symbol}";
		public Currency BaseAsset { get; }
		public Currency QuoteAsset { get; }

		public override bool Equals(object obj)
		{
			return Equals((Pair)obj);
		}

		public override int GetHashCode()
		{
			return BaseAsset.Id ^ QuoteAsset.Id;
		}

		public bool Equals(Pair other)
		{
			if (other == null)
			{
				return false;
			}

			return BaseAsset.Id == other.BaseAsset.Id && QuoteAsset.Id == other.QuoteAsset.Id;
		}
	}
}