using System;

namespace TradingBot.CommandPrompt
{
	using System.Threading.Tasks;
	using Yobit.Exchange.Api;

	public class Program
    {
        private async static Task Main(string[] args)
        {
	        var client = new YobitClient();
			Console.WriteLine(client.GetPairData("ltc_btc"));

	        Console.ReadLine();
        }
    }
}