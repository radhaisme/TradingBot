
using System.Threading.Tasks;
using Kucoin.Api;

namespace TradingBot.CommandPrompt
{
	using System;
	using System.IO;
	using System.Linq;
	using Binance.Api;
	using Core;
	using Core.Enums;
	using Huobi.Api;
	using Yobit.Api;

	public class Program
	{
		private static async Task Main(string[] args)
		{
			var api = new KucoinApi("https://api.kucoin.com", "https://api.kucoin.com");
			var r = await api.GetPairDetail("KCS-BTC");


			//var api = new HuobiApi("https://api.huobi.pro", "https://api.huobi.pro");
			//var r = api.GetCurrencies().Result;

			//var api = new BinanceApi("https://api.binance.com/api", "https://api.binance.com/api/v3");
			//var r = api.Time().Result;

			//var client = new YobitClient("https://yobit.net/api/3/", "https://yobit.net/tapi/", new YobitSettings
			//{
			//	Secret = "5ceeeb6072789d30e79a961335e63d50",
			//	ApiKey = "B03E731C650825B49CB2840E8449D98D",
			//	CreatedAt = new DateTime(2018, 1, 1)
			//});
			//var r = client.GetPairs();

			Console.ReadLine();
		}
	}
}