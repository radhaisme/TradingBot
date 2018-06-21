using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;

namespace Cryptopia.Api
{
	public sealed class CryptopiaApi : ExchangeApi
	{
		public CryptopiaApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<string> GetCurrencies()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "GetCurrencies");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}

		public async Task<string> GetTradePairs()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "GetTradePairs");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}

		public async Task<string> GetMarket(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"GetMarket/{pair}");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}
	}
}