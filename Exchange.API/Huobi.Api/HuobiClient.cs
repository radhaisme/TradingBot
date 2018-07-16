using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Huobi.Api
{
	public sealed class HuobiClient : ApiClient, IExchangeClient
	{
		private readonly IHuobiSettings _settings;

		public HuobiClient()
		{
			_settings = new HuobiSettings();
		}

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "v1/common/symbols"));
			var pairs = new List<PairDto>();

			foreach (dynamic item in content.data)
			{
				var dto = new PairDto();
				dto.BaseAsset = ((string)item["base-currency"]).ToUpper();
				dto.QuoteAsset = ((string)item["quote-currency"]).ToUpper();
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

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"market/detail/merged?symbol={pair}"));
			var dto = new PairDetailDto();
			dto.LastPrice = content.tick.close;
			dto.Ask = content.tick.ask[0];
			dto.Bid = content.tick.bid[0];
			dto.Volume = content.tick.vol;
			dto.High = content.tick.high;
			dto.Low = content.tick.low;

			return dto;
		}

		public async Task<OrderBookDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"market/depth?symbol={pair}&type=step1"));
			OrderBookDto model = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.tick.asks).Take((int)limit), ((IEnumerable<dynamic>)content.tick.bids).Take((int)limit), item => new OrderDto { Price = item[0], Amount = item[1] });

			return model;
		}

		//public async Task<IReadOnlyCollection<PairDetailDto>> GetPairsDetails(params string[] pairs)
		//{
		//	var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairsDetailsAsync());
		//	var details = new List<PairDetailDto>();

		//	foreach (dynamic item in content.data)
		//	{
		//		var dto = new PairDetailDto();
		//		dto.LastPrice = item.close;
		//		//detail.Low = item.low;
		//		//detail.High = item.high;
		//		//detail.Volume = item.vol;
		//		details.Add(dto);
		//	}

		//	return new ReadOnlyCollection<PairDetailDto>(details);
		//}
	}
}