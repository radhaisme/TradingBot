using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Binance.Api
{
	public sealed class BinanceClient : ApiClient, IExchangeClient
	{
		private readonly IBinanceSettings _settings;

		public BinanceClient()
		{
			_settings = new BinanceSettings();
		}

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "exchangeInfo"));
			var pairs = new List<PairDto>();

			foreach (dynamic item in content.symbols)
			{
				var dto = new PairDto();
				dto.BaseAsset = item.baseAsset;
				dto.QuoteAsset = item.quoteAsset;
				pairs.Add(dto);
			}

			return pairs.AsReadOnly();
		}

		public async Task<PairDetailDto> GetPairDetailAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"ticker/price?symbol={pair}"));
			var dto = new PairDetailDto();
			dto.LastPrice = content.price;

			return dto;
		}

		public async Task<OrderBookDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"depth?symbol={pair}&limit={limit}"));
			var dto = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.asks).Take((int)limit), ((IEnumerable<dynamic>)content.bids).Take((int)limit), item => new OrderDto { Price = item[0], Amount = item[1] });

			return dto;
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
			{
				return;
			}

			var content = await HttpHelper.AcquireContentAsync<ErrorModel>(response);
			throw new HttpRequestException(content.Msg);
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