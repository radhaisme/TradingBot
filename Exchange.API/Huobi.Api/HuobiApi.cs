using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Huobi.Api
{
	internal sealed class HuobiApi : ExchangeApi
	{
		public HuobiApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> GetPairs()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "common/symbols");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairDetail(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"market/detail/merged?symbol={pair}");

			return response.EnsureSuccessStatusCode();
		}
	}
}