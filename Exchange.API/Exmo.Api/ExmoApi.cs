using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Exmo.Api
{
	internal class ExmoApi : ExchangeApi
	{
		public ExmoApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> GetPairsDetails()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "ticker");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairs()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "pair_settings");

			return response.EnsureSuccessStatusCode();
		}
	}
}