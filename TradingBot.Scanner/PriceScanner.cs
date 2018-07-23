using TradingBot.Core;

namespace TradingBot.Scanner
{
	public sealed class PriceScanner
	{
		private readonly IExchangeClient _client;
		
		public PriceScanner(IExchangeClient client)
		{
			_client = client;
		}


	}
}