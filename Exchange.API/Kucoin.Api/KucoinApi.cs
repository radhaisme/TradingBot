using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Kucoin.Api
{
	internal sealed class KucoinApi : ExchangeApi
	{
		public KucoinApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> GetCurrencies()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "market/open/coins");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairs()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "market/open/symbols");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairDetail(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"{pair}/open/tick");

			return response.EnsureSuccessStatusCode();
		}
	}
}