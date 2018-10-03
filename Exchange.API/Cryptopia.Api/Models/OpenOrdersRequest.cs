
namespace Cryptopia.Api.Models
{
	public sealed class OpenOrdersRequest
	{
		public OpenOrdersRequest(string pair)
		{
			Market = pair;
		}

		public string Market { get; }
	}
}