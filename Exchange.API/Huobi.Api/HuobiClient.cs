using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace Huobi.Api
{
	public sealed class HuobiClient : ApiClient, IApiClient
	{
		private readonly IHuobiSettings _settings;

		public HuobiClient()
		{
			_settings = new HuobiSettings();
		}

		public ExchangeType Type => ExchangeType.Huobi;

		public async Task<PairResponse> GetPairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "v1/common/symbols"));
			var pairs = new List<TradePair>();

			foreach (dynamic item in content.data)
			{
				var dto = new TradePair();
				dto.BaseAsset = ((string)item["base-currency"]).ToUpper();
				dto.QuoteAsset = ((string)item["quote-currency"]).ToUpper();
				pairs.Add(dto);
			}

			return new PairResponse(pairs);
		}

		public async Task<PairDetailResponse> GetPairDetailAsync(PairDetailRequest request)
		{
			if (String.IsNullOrEmpty(request.Pair))
			{
				throw new ArgumentNullException(nameof(request.Pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"market/detail/merged?symbol={request.Pair}"));
			var dto = new PairDetailResponse();
			dto.LastPrice = content.tick.close;
			dto.Ask = content.tick.ask[0];
			dto.Bid = content.tick.bid[0];
			dto.Volume = content.tick.vol;
			dto.High = content.tick.high;
			dto.Low = content.tick.low;

			return dto;
		}

		public async Task<DepthResponse> GetOrderBookAsync(DepthRequest request)
		{
			//if (String.IsNullOrEmpty(request.Pair))
			//{
			//	throw new ArgumentNullException(nameof(request.Pair));
			//}

			//var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"market/depth?symbol={request.Pair}&type=step1"));
			//DepthResponse model = Helper.BuildOrderBook(((IEnumerable<dynamic>)content.tick.asks).Take((int)request.Limit), ((IEnumerable<dynamic>)content.tick.bids).Take((int)request.Limit), item => new BookOrderDto { Price = item[0], Amount = item[1] });

			//return model;

			return null;
		}

		public Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<CancelOrderResponse> CancelOrderAsync(CancelOrderRequest request)
		{
			throw new NotImplementedException();
		}

		//public async Task<IReadOnlyCollection<PairDetailResponse>> GetPairsDetails(params string[] pairs)
		//{
		//	var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairsDetailsAsync());
		//	var details = new List<PairDetailResponse>();

		//	foreach (dynamic item in content.data)
		//	{
		//		var dto = new PairDetailResponse();
		//		dto.LastPrice = item.close;
		//		//detail.Low = item.low;
		//		//detail.High = item.high;
		//		//detail.Volume = item.vol;
		//		details.Add(dto);
		//	}

		//	return new ReadOnlyCollection<PairDetailResponse>(details);
		//}
	}
}