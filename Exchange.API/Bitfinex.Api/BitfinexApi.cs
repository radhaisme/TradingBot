using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Bitfinex.Api
{
	internal sealed class BitfinexApi : ExchangeApi
	{
		public BitfinexApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> GetPairsAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "symbols");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairsDetailsAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "symbols_details");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairDetailAsync(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"pubticker/{pair}");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetOrderBookAsync(string pair, uint limit)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"book/{pair}?limit_bids={limit}&limit_asks={limit}");

			return response.EnsureSuccessStatusCode();
		}
	}
}