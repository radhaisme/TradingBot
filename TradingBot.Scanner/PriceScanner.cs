using TradingBot.Core;

namespace TradingBot.Scanner
{
	public sealed class PriceScanner
	{
		private readonly IApiClient _client;
		
		public PriceScanner(IApiClient client)
		{
			_client = client;
		}


	}
}