using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace Okex.Api
{
	public sealed class OkexClient : ApiClient, IApiClient
	{
		private readonly IOkexSettings _settings;

		public OkexClient()
		{
			_settings = new OkexSettings();
		}

		public ExchangeType Type => _settings.Type;

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "tickers.do"));
			var pairs = new List<PairDto>();

			foreach (dynamic item in content.tickers)
			{
				var dto = new PairDto();
				string[] assets = ((string)item.symbol).Split('_');
				dto.BaseAsset = assets[0].ToUpper();
				dto.QuoteAsset = assets[1].ToUpper();
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

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"ticker.do?symbol={pair}"));
			var dto = new PairDetailDto();
			dto.LastPrice = content.ticker.last;
			dto.Ask = content.ticker.buy;
			dto.Bid = content.ticker.sell;
			dto.High = content.ticker.high;
			dto.Low = content.ticker.low;
			dto.Volume = content.ticker.vol;

			return dto;
		}

		public async Task<DepthDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"depth.do?symbol={pair}&size={limit}"));
			var model = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.asks).Take((int)limit), ((IEnumerable<dynamic>)content.bids).Take((int)limit), item => new BookOrderDto { Price = item[0], Amount = item[1] });

			return model;
		}

		public Task<CreateOrderDto> CreateOrderAsync(OrderDto input)
		{

			return null;
		}

		public Task<CancelOrderDto> CancelOrderAsync(CancelOrderDto input)
		{


			return null;
		}

		#region Private methods

		private Task<dynamic> MakePrivateCallAsync(string content)
		{
			if (String.IsNullOrEmpty(_settings.PrivateUrl))
			{
				throw new ArgumentNullException(nameof(_settings.PrivateUrl));
			}

			if (String.IsNullOrEmpty(_settings.ApiKey))
			{
				throw new ArgumentNullException(nameof(_settings.ApiKey));
			}

			if (String.IsNullOrEmpty(_settings.Secret))
			{
				throw new ArgumentNullException(nameof(_settings.Secret));
			}

			SetHeaders(new Dictionary<string, string>
			{
				{"Key", _settings.ApiKey},
				{
					"Sign", BitConverter.ToString(new HMACSHA512(Encoding.UTF8.GetBytes(_settings.Secret)).ComputeHash(Encoding.UTF8.GetBytes(content))).Replace("-", "").ToLower()
				}
			});

			return CallAsync<dynamic>(HttpMethod.Post, BuildUrl(_settings.PrivateUrl, content), new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			var content = await HttpHelper.AcquireContentAsync<ErrorModel>(response);

			if (content.ErrorCode > 0)
			{
				string message = content.GetMessage();
				throw new HttpRequestException(message);
			}
		}

		#endregion
	}
}