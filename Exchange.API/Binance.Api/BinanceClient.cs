using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace Binance.Api
{
	public sealed class BinanceClient : ApiClient, IApiClient
	{
		private readonly IBinanceSettings _settings;
		private readonly uint[] _limits = { 5, 10, 20, 50, 100, 500, 1000 };

		public BinanceClient()
		{
			_settings = new BinanceSettings();
		}

		public ExchangeType Type => _settings.Type;

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

		public async Task<DepthDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			if (!_limits.Contains(limit))
			{
				throw new ArgumentOutOfRangeException(nameof(limit));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"depth?symbol={pair}&limit={limit}"));
			var dto = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.asks).Take((int)limit), ((IEnumerable<dynamic>)content.bids).Take((int)limit), item => new BookOrderDto { Price = item[0], Amount = item[1] });

			return dto;
		}

		public async Task<CreateOrderDto> CreateOrderAsync(OrderDto input)
		{
			var queryString = HttpHelper.QueryString(new Dictionary<string, string>
			{
				{ "symbol", input.Pair },
				{ "side", input.Side.ToString().ToUpper() },
				{ "type", input.Type.ToString().ToUpper() },
				{ "quantity", input.Amount.ToString(CultureInfo.InvariantCulture) },
				{ "price", input.Price.ToString(CultureInfo.InvariantCulture) },
				{ "timeInForce", "GTC" },
				{ "timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() }
			}, true);
			var content = await MakePrivateCallAsync(HttpMethod.Post, "order", queryString);
			var dto = new CreateOrderDto();
			dto.OrderId = content.orderId;

			return dto;
		}

		public async Task<CancelOrderDto> CancelOrderAsync(CancelOrderDto input)
		{
			if (input.OrderId <= 0)
			{
				throw new ArgumentException(nameof(input.OrderId));
			}

			var queryString = HttpHelper.QueryString(new Dictionary<string, string>
			{
				{ "symbol", input.Pair },
				{ "orderId", input.OrderId.ToString() },
				{ "timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() }
			}, true);
			var content = await MakePrivateCallAsync(HttpMethod.Delete, "order", queryString);
			var dto = new CancelOrderDto();
			dto.OrderId = content.orderId;

			return dto;
		}

		#region Private methods

		private Task<dynamic> MakePrivateCallAsync(HttpMethod method, string url, string content)
		{
			SetHeaders(new Dictionary<string, string> { { "X-MBX-APIKEY", _settings.ApiKey } });
			string hash = HttpHelper.GetHash(new HMACSHA256(), _settings.Secret, content);
			content += $"&signature={hash}";

			return CallAsync<dynamic>(method, BuildUrl(_settings.PrivateUrl, url), new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));
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

		#endregion
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