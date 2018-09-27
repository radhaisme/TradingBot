
namespace Binance.Api.Models
{
	public sealed class OpenOrdersRequest
	{
		public OpenOrdersRequest(string pair)
		{
			Pair = pair;
		}

		public string Pair { get; }
	}
}