﻿using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;

namespace Binance.Api
{
	internal class BinanceApi : ExchangeApi
	{
		public BinanceApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		public async Task<bool> Ping()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "ping");

			return response.IsSuccessStatusCode;
		}

		public async Task<string> Time()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "time");

			if (response.IsSuccessStatusCode)
			{
				return await HttpHelper.AcquireStringAsync(response);
			}

			return null;
		}

		public async Task<HttpResponseMessage> GetPairs()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "exchangeInfo");
			
			return response.EnsureSuccessStatusCode();
		}

		public async Task<HttpResponseMessage> GetPairDetail(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"ticker/price?symbol={pair}");

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