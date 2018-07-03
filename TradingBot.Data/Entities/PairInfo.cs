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

		public Currency BaseAsset { get; }
		public Currency QuoteAsset { get; }

		public bool Equals(Pair x, Pair y)
		{
			if (x == null || y == null)
			{
				return false;
			}

			return x.BaseAsset.Id == y.BaseAsset.Id && x.QuoteAsset.Id == y.QuoteAsset.Id;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Pair) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((BaseAsset != null ? BaseAsset.GetHashCode() : 0) * 397) ^ (QuoteAsset != null ? QuoteAsset.GetHashCode() : 0);
			}
		}

		public int GetHashCode(Pair obj)
		{
			return GetHashCode() ^ obj.GetHashCode();
		}

		public bool Equals(Pair other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(BaseAsset, other.BaseAsset) && Equals(QuoteAsset, other.QuoteAsset);
		}
	}
}