using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Okex.Api
{
	internal sealed class OkexApi : ExchangeApi
	{
		public OkexApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> GetPairs()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "tickers.do");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairDetail(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"ticker.do?symbol={pair}");

			return response.EnsureSuccessStatusCode();
		}
	}
}