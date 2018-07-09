using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Okex.Api
{
	internal sealed class OkexApi : ExchangeApi
	{
		public OkexApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> GetPairsAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "tickers.do");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairDetailAsync(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"ticker.do?symbol={pair}");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetOrderBookAsync(string pair, uint limit)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"depth.do?symbol={pair}&size={limit}");

			return response.EnsureSuccessStatusCode();
		}
	}
}