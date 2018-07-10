using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Huobi.Api
{
	internal sealed class HuobiApi : ExchangeApi
	{
		public HuobiApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> GetPairsAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "v1/common/symbols");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairDetailAsync(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"market/detail/merged?symbol={pair}");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairsDetailsAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "market/tickers");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetOrderBookAsync(string pair, uint limit)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"market/depth?symbol={pair}&type=step1");

			return response.EnsureSuccessStatusCode();
		}
	}
}