
namespace Yobit.Api.Entities
{
	using System.Collections.Generic;

	public class TradeInfo
	{
		public TradeInfo()
		{
			Trades = new List<Trade>();
		}

		public ICollection<Trade> Trades { get; set; }
	}
}