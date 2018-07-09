using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Huobi.Api
{
	public sealed class HuobiClient : IExchangeClient
	{
		private readonly HuobiApi _api;
		private readonly IHuobiSettings _settings;

		public HuobiClient()
		{
			_settings = new HuobiSettings();
			_api = new HuobiApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairsAsync());
			var pairs = new List<PairDto>();

			foreach (dynamic item in content.data)
			{
				var pair = new PairDto();
				pair.BaseAsset = ((string)item["base-currency"]).ToUpper();
				pair.QuoteAsset = ((string)item["quote-currency"]).ToUpper();
				//pair.Precision = item["price-precision"];
				pairs.Add(pair);
			}

			return new ReadOnlyCollection<PairDto>(pairs);
		}

		public async Task<PairDetailDto> GetPairDetailAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairDetailAsync(pair));
			var detail = new PairDetailDto();
			detail.LastPrice = content.tick.close;
			detail.Ask = content.tick.ask[0];
			detail.Bid = content.tick.bid[0];
			detail.Volume = content.tick.vol;
			detail.High = content.tick.high;
			detail.Low = content.tick.low;

			return detail;
		}

		public async Task<OrderBookDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetOrderBookAsync(pair, limit));
			var model = new OrderBookDto();

			foreach (dynamic item in content.tick.bids)
			{
				var dto = new OrderDto();
				dto.Price = item[0];
				dto.Amount = item[1];
				model.Bids.Add(dto);
			}

			foreach (dynamic item in content.tick.asks)
			{
				var dto = new OrderDto();
				dto.Price = item[0];
				dto.Amount = item[1];
				model.Asks.Add(dto);
			}

			return model;
		}

		public async Task<IReadOnlyCollection<PairDetailDto>> GetPairsDetails(params string[] pairs)
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairsDetailsAsync());
			var details = new List<PairDetailDto>();

			foreach (dynamic item in content.data)
			{
				var detail = new PairDetailDto();
				detail.LastPrice = item.close;
				//detail.Low = item.low;
				//detail.High = item.high;
				//detail.Volume = item.vol;
				details.Add(detail);
			}

			return new ReadOnlyCollection<PairDetailDto>(details);
		}
	}
}