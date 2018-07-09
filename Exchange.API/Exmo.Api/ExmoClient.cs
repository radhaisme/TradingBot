using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Exmo.Api
{
	public class ExmoClient : IExchangeClient
	{
		private readonly ExmoApi _api;
		private readonly IExmoSettings _settings;

		public ExmoClient()
		{
			_settings = new ExmoSettings();
			_api = new ExmoApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await HttpHelper.AcquireContentAsync<Dictionary<string, dynamic>>(await _api.GetPairsAsync());
			var pairs = new List<PairDto>();

			foreach (string key in content.Keys)
			{
				//dynamic item = content[key];
				var pair = new PairDto();
				string[] assets = key.Split('_');
				pair.BaseAsset = assets[0];
				pair.QuoteAsset = assets[1];
				//pair.MinOrderSize = item.min_quantity;
				//pair.MaxOrderSize = item.max_quantity;
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

			var content = await HttpHelper.AcquireContentAsync<Dictionary<string, dynamic>>(await _api.GetPairsDetailsAsync());
			var model = new PairDetailDto();
			dynamic item = content[pair];
			model.LastPrice = item.last_trade;
			model.High = item.high;
			model.Low = item.low;
			model.Volume = item.vol;
			model.Ask = item.buy_price;
			model.Bid = item.sell_price;

			return model;
		}

		public async Task<OrderBookDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetOrderBookAsync(pair, limit));
			var model = new OrderBookDto();

			foreach (dynamic item in content[pair].ask)
			{
				var dto = new OrderDto();
				dto.Price = item[0];
				dto.Amount = item[2];
				model.Asks.Add(dto);
			}

			foreach (dynamic item in content[pair].bid)
			{
				var dto = new OrderDto();
				dto.Price = item[0];
				dto.Amount = item[2];
				model.Bids.Add(dto);
			}

			return model;
		}
	}
}