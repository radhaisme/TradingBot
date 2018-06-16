
namespace Binance.Api
{
	using System.Net.Http;
	using System.Threading.Tasks;
	using TradingBot.Common;
	using TradingBot.Core;

	public class BinanceApi : ExchangeApi
	{
		public BinanceApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<bool> Ping()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "/ping");

			return response.IsSuccessStatusCode;
		}

		public async Task<string> Time()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "/time");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}

		public async Task<string> Info()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "/exchangeInfo");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}
	}
}