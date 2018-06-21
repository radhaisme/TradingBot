using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;

namespace Kucoin.Api
{
	public class KucoinApi : ExchangeApi
	{
		public KucoinApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<string> GetCurrencies()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "market/open/coins");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}

		public async Task<string> GetPairs()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "market/open/symbols");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}

		public async Task<string> GetPairDetail(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"{pair}/open/tick");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}
	}
}