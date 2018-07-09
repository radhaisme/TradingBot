using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Okex.Api
{
	public sealed class OkexClient : IExchangeClient
	{
		private readonly OkexApi _api;
		private readonly IOkexSettings _settings;

		public OkexClient()
		{
			_settings = new OkexSettings();
			_api = new OkexApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairsAsync());
			var pairs = new List<PairDto>();

			foreach (dynamic item in content.tickers)
			{
				var pair = new PairDto();
				string[] assets = ((string)item.symbol).Split('_');
				pair.BaseAsset = assets[0].ToUpper();
				pair.QuoteAsset = assets[1].ToUpper();
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
			var dto = new PairDetailDto();
			dto.LastPrice = content.ticker.last;
			dto.Ask = content.ticker.buy;
			dto.Bid = content.ticker.sell;
			dto.High = content.ticker.high;
			dto.Low = content.ticker.low;
			dto.Volume = content.ticker.vol;

			return dto;
		}

		public async Task<OrderBookDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetOrderBookAsync(pair, limit));
			var model = new OrderBookDto();

			foreach (dynamic item in content.asks)
			{
				var dto = new OrderDto();
				dto.Price = item[0];
				dto.Amount = item[1];
				model.Asks.Add(dto);
			}

			foreach (dynamic item in content.bids)
			{
				var dto = new OrderDto();
				dto.Price = item[0];
				dto.Amount = item[1];
				model.Bids.Add(dto);
			}

			return model;
		}
	}
}