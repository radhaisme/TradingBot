using Binance.Api;
using Xunit;

namespace TradingBot.Tests
{
	public class BinanceTest
	{
		private readonly BinanceClient _client = new BinanceClient();

		[Fact]
		public async void GetTradePairsTest()
		{
			//var response = await _client.GetTradePairsAsync();
			//Assert.NotNull(response);
			//Assert.NotEmpty(response.Pairs);
		}

		[Fact]
		public async void GetOrderBookTest()
		{
			//var book = await _client.GetOrderBookAsync(new DepthRequest { Pair = "ETHBTC", Limit = 100 });
			//Assert.NotNull(book);
			//Assert.NotEmpty(book.Asks);
			//Assert.NotEmpty(book.Bids);
		}

		[Fact]
		public async void GetPairDetailTest()
		{
			//PairDetailResponse detail = await _client.GetMarketAsync(new PairDetailRequest { Pair = "ETHBTC" });
			//Assert.NotNull(detail);
			//Assert.NotEqual(0, detail.LastPrice);
		}
	}
}