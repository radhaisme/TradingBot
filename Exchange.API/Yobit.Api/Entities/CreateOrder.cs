
namespace Yobit.Api.Entities
{
	using System.Collections.Generic;

	public class CreateOrder
	{
		public CreateOrder()
		{
			Funds = new Dictionary<string, decimal>();
		}

		public decimal Received { get; set; }
		public decimal Remains { get; set; }
		public int OrderId { get; set; }
		public IDictionary<string, decimal> Funds { get; set; }
	}
}