using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace TradingBot.CurrencyProvider
{
	internal sealed class CoinMarketCapApi : ExchangeApi
	{
		public CoinMarketCapApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> GetCurrenciesAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "listings");

			return response.EnsureSuccessStatusCode();
		}
	}
}