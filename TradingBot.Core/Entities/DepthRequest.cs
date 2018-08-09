using System;
using System.Collections.Generic;
using System.Text;

namespace TradingBot.Core.Entities
{
	public class DepthRequest
	{
		public string Pair { get; set; }
		public uint Limit { get; set; }
	}
}