using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;

namespace Binance.Api
{
	internal sealed class BinanceApi : ExchangeApi
	{
		public BinanceApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<HttpResponseMessage> Ping()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "ping");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> Time()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "time");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairsAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "exchangeInfo");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairDetailAsync(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"ticker/price?symbol={pair}");

			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetOrderBookAsync(string pair, uint limit)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"depth?symbol={pair}&limit={limit}");

			return response.EnsureSuccessStatusCode();
		}
	}

	public static class Interval
	{
		public const string OneM = "1m";
		public const string ThreeM = "3m";
		public const string FiveM = "5m";
		public const string FifteenM = "15m";
		public const string ThirteenM = "30m";
		public const string OneH = "1h";
		public const string TwoH = "2h";
		public const string FourH = "4h";
		public const string SixH = "6h";
		public const string EightH = "8h";
		public const string TwelveH = "12h";
		public const string OneD = "1d";
		public const string ThreeD = "3d";
		public const string OneW = "1w";
		public const string OneMn = "1M";
	}
}