using System.Collections.Generic;

namespace TradingBot.Core.Models
{
	public class Balance
	{
		public Balance()
		{
			Funds = new Dictionary<string, decimal>();
			FundsIncludeOrders = new Dictionary<string, decimal>();
		}

		public IDictionary<string, decimal> Funds { get; }
		public IDictionary<string, decimal> FundsIncludeOrders { get; set; }
		public int TransactionCount { get; set; }
		public int OpenOrders { get; set; }
	}
}