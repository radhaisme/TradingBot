using Binance.Api;
using System.Collections.Generic;
using TradingBot.Core;
using TradingBot.Core.Entities;
using Xunit;

namespace TradingBot.Tests
{
	public class BinanceTest
	{
		private readonly IExchangeClient _client = new BinanceClient();

		[Fact]
		public async void GetPairsTest()
		{
			IReadOnlyCollection<PairDto> pairs = await _client.GetPairsAsync();
			Assert.NotNull(pairs);
			Assert.NotEmpty(pairs);
		}

		[Fact]
		public async void GetOrderBookTest()
		{
			OrderBookDto book = await _client.GetOrderBookAsync("ETHBTC");
			Assert.NotNull(book);
			Assert.NotEmpty(book.Asks);
			Assert.NotEmpty(book.Bids);
		}

		[Fact]
		public async void GetPairDetail()
		{
			PairDetailDto detail = await _client.GetPairDetailAsync("ETHBTC");
			Assert.NotNull(detail);
			Assert.NotEqual(0, detail.LastPrice);
		}
	}
}