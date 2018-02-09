using System;

namespace TradingBot.CommandPrompt
{
	using Yobit.Exchange.Api;

	public class Program
    {
        private static void Main(string[] args)
        {
	        var client = new YobitClient();
			Console.WriteLine(client.GetPairData("ltc_btc"));

	        Console.ReadLine();
        }
    }
}