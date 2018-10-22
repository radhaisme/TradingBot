using TradingBot.Api;
using TradingBot.Core.Proxies;

namespace TradingBot.Scanner
{
	public sealed class PriceScanner
	{
		private readonly IProxy _client;
		
		public PriceScanner(IProxy client)
		{
			_client = client;
		}
	}
}