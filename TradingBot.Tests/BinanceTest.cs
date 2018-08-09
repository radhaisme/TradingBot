using Binance.Api;
using System.Collections.Generic;
using TradingBot.Core;
using TradingBot.Core.Entities;
using Xunit;

namespace TradingBot.Tests
{
	public class BinanceTest
	{
		private readonly IApiClient _client = new BinanceClient();

		[Fact]
		public async void GetPairsTest()
		{
			PairResponse response = await _client.GetPairsAsync();
			Assert.NotNull(response);
			Assert.NotEmpty(response.Pairs);
		}

		[Fact]
		public async void GetOrderBookTest()
		{
			DepthResponse book = await _client.GetOrderBookAsync(new DepthRequest { Pair = "ETHBTC", Limit = 100 });
			Assert.NotNull(book);
			Assert.NotEmpty(book.Asks);
			Assert.NotEmpty(book.Bids);
		}

		[Fact]
		public async void GetPairDetailTest()
		{
			PairDetailResponse detail = await _client.GetPairDetailAsync(new PairDetailRequest { Pair = "ETHBTC" });
			Assert.NotNull(detail);
			Assert.NotEqual(0, detail.LastPrice);
		}
	}
}