
namespace Yobit.Api.Entities
{
	using System.Collections.Generic;

	public class PairOrders
	{
		public PairOrders()
		{
			Asks = new List<Order>();
			Bids = new List<Order>();
		}

		public ICollection<Order> Asks { get; set; }
		public ICollection<Order> Bids { get; set; }
	}
}