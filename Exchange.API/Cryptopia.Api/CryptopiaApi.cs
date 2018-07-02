using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Cryptopia.Api
{
	internal sealed class CryptopiaApi : ExchangeApi
	{
		public CryptopiaApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> GetPairs()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "GetPairsAsync");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetTradePairs()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "GetTradePairs");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairDetail(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"GetMarket/{pair}");

			return response.EnsureSuccessStatusCode();
		}
	}
}