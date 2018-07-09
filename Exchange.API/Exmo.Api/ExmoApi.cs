using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Exmo.Api
{
	internal sealed class ExmoApi : ExchangeApi
	{
		public ExmoApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> GetPairsDetailsAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "ticker");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairsAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "pair_settings");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetOrderBookAsync(string pair, uint limit)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"order_book?pair={pair}&limit={limit}");

			return response.EnsureSuccessStatusCode();
		}
	}
}