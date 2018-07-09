using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Kucoin.Api
{
	public sealed class KucoinClient : IExchangeClient
	{
		private readonly KucoinApi _api;
		private readonly IKucoinSettings _settings;

		public KucoinClient()
		{
			_settings = new KucoinSettings();
			_api = new KucoinApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairsAsync());
			var pairs = new List<PairDto>();

			foreach (dynamic item in content.data)
			{
				if (!(bool)item.trading)
				{
					continue;
				}

				var pair = new PairDto();
				pair.BaseAsset = item.coinType;
				pair.QuoteAsset = item.coinTypePair;
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

			if (!(bool)content.data.trading)
			{
				return null;
			}

			var detail = new PairDetailDto();
			detail.LastPrice = content.data.lastDealPrice;
			detail.Ask = content.data.buy;
			detail.Bid = content.data.sell;
			detail.Volume = content.data.vol;
			detail.Low = ((decimal?)content.data.low).GetValueOrDefault();
			detail.High = ((decimal?)content.data.high).GetValueOrDefault();

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

			foreach (dynamic item in content.data.SELL)
			{
				var dto = new OrderDto();
				dto.Price = item[0];
				dto.Amount = item[1];
				model.Bids.Add(dto);
			}

			foreach (dynamic item in content.data.BUY)
			{
				var dto = new OrderDto();
				dto.Price = item[0];
				dto.Amount = item[1];
				model.Asks.Add(dto);
			}

			return model;
		}
	}
}