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
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}

		public async Task<string> GetPairDetail(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"/future_ticker.do?symbol={pair}&contract_type=this_week");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}
	}
}