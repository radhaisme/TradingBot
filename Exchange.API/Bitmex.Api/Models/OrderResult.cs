using System;

namespace Bitmex.Api.Models
{
	public sealed class OrderResult
	{
		public OrderResult(string pair)
		{
			Pair = pair;
		}

		//public long OrderId { get; }
		//public DateTime TimeStamp { get; set; }
		//public decimal? Leverage { get; set; }
		//public int? CurrentQty { get; set; }
		//public decimal? CurrentCost { get; set; }
		//public bool IsOpen { get; set; }
		//public decimal? MarkPrice { get; set; }
		//public decimal? MarkValue { get; set; }
		//public decimal? UnrealisedPnl { get; set; }
		//public decimal? UnrealisedPnlPcnt { get; set; }
		//public decimal? UnrealisedRoePcnt { get; set; }
		//public decimal? AvgEntryPrice { get; set; }
		//public decimal? BreakEvenPrice { get; set; }
		//public decimal? LiquidationPrice { get; set; }
		//public decimal? RealizedPnl { get; set; }
		//public decimal? HighestPriceSinceOpen { get; set; }
		//public decimal? LowestPriceSinceOpen { get; set; }
		//public decimal? TrailingStopPrice { get; set; }



		//public decimal? UsefulUnrealisedPnl
		//{
		//	get
		//	{
		//		if (UnrealisedPnl != null)
		//		{
		//			return Math.Round(((decimal)UnrealisedPnl / 100000000), 4);
		//		}

		//		return 0;
		//	}
		//}

		//public TradeType TradeType { get; set; }
		//public OrderType OrderType { get; set; }
		public string Pair { get; }
		public decimal Rate { get; set; }
		public decimal Amount { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}