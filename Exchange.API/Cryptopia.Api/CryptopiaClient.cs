using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace Cryptopia.Api
{
	public sealed class CryptopiaClient : ApiClient, IExchangeClient
	{
		private readonly ICryptopiaSettings _settings;

		public CryptopiaClient()
		{
			_settings = new CryptopiaSettings();
		}

		public ExchangeType Type => _settings.Type;

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "GetTradePairs"));
			var pairs = new List<PairDto>();

			foreach (dynamic item in content.Data)
			{
				var dto = new PairDto();
				dto.BaseAssetName = item.Currency;
				dto.BaseAsset = item.Symbol;
				dto.QuoteAssetName = item.BaseCurrency;
				dto.QuoteAsset = item.BaseSymbol;
				dto.MaxOrderSize = item.MaximumTrade;
				dto.MinOrderSize = item.MinimumTrade;
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

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"GetMarket/{pair}"));
			var dto = new PairDetailDto();
			dto.Ask = content.Data.AskPrice;
			dto.Bid = content.Data.BidPrice;
			dto.High = content.Data.High;
			dto.Low = content.Data.Low;
			dto.LastPrice = content.Data.LastPrice;
			dto.Volume = content.Data.Volume;

			return dto;
		}

		public async Task<OrderBookDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"GetMarketOrders/{pair}/{limit}"));
			var dto = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.Data.Buy).Take((int)limit), ((IEnumerable<dynamic>)content.Data.Sell).Take((int)limit), item => new OrderDto { Price = item.Price, Amount = item.Volume });

			return dto;
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			var content = await HttpHelper.AcquireContentAsync<ResponseModel>(response);

			if (!String.IsNullOrEmpty(content.Error))
			{
				throw new HttpRequestException(content.Error);
			}
		}
	}
}