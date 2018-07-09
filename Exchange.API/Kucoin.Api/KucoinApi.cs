using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Kucoin.Api
{
	internal sealed class KucoinApi : ExchangeApi
	{
		public KucoinApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> GetCurrenciesAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "market/open/coins");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairsAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "market/open/symbols");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairDetailAsync(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"{pair}/open/tick");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetOrderBookAsync(string pair, uint limit)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"open/orders?symbol={pair}&limit={limit}");

			return response.EnsureSuccessStatusCode();
		}
	}
}