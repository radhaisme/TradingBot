using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Bitfinex.Api
{
	internal class BitfinexApi : ExchangeApi
	{
		public BitfinexApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> GetPairs()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "symbols");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairsDetails()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "symbols_details");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairDetail(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"pubticker/{pair}");

			return response.EnsureSuccessStatusCode();
		}
	}
}