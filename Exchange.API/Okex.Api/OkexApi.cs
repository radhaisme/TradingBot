using System;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;

namespace Okex.Api
{
	public class OkexApi : ExchangeApi
	{
		public OkexApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<string> GetCurrencies()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(new Uri(PublicUrl.OriginalString.Substring(0, PublicUrl.OriginalString.Length - 7) + "v2/") + "spot/markets/products");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}

		public async Task<string> GetPairDetail(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"ticker.do?symbol={pair}");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}
	}
}