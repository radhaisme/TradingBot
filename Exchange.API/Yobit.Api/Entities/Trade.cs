
namespace Yobit.Api.Entities
{
	using System;

	public class Trade
	{
		public TradeType Type { get; set; }
		public decimal Price { get; set; }
		public decimal Amount { get; set; }
		public int Tid { get; set; }
		public DateTimeOffset Timestamp { get; set; }
	}
}