using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Bitfinex.Api
{
	public sealed class BitfinexClient : IExchangeClient
	{
		private readonly BitfinexApi _api;
		private readonly IBitfinexSettings _settings;

		public BitfinexClient()
		{
			_settings = new BitfinexSettings();
			_api = new BitfinexApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			HttpResponseMessage[] results = await Task.WhenAll(_api.GetPairsAsync(), _api.GetPairsDetailsAsync());
			var items = await HttpHelper.AcquireContentAsync<string[]>(results[0]);
			var details = await HttpHelper.AcquireContentAsync<dynamic>(results[1]);
			var pairs = new Dictionary<string, PairDto>(items.Length);

			foreach (var item in items)
			{
				var pair = new PairDto();
				pair.BaseAsset = item.Substring(0, item.Length - 3);
				pair.QuoteAsset = item.Substring(item.Length - 3, 3);
				pairs.Add(item, pair);
			}

			foreach (dynamic detail in details)
			{
				if (!pairs.ContainsKey((string)detail.pair))
				{
					continue;
				}

				PairDto pairDto = pairs[(string)detail.pair];
				pairDto.Precision = detail.price_precision;
				pairDto.MaxOrderSize = detail.maximum_order_size;
				pairDto.MinOrderSize = detail.minimum_order_size;
			}

			return new ReadOnlyCollection<PairDto>(pairs.Values.ToList());
		}

		public async Task<PairDetailDto> GetPairDetailAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairDetailAsync(pair));
			var detail = new PairDetailDto();
			detail.LastPrice = content.last_price;
			detail.Ask = content.ask;
			detail.Bid = content.bid;
			detail.Volume = content.volume;
			detail.High = content.high;
			detail.Low = content.low;

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

			foreach (dynamic item in content.asks)
			{
				var dto = new OrderDto();
				dto.Price = item.price;
				dto.Amount = item.amount;
				model.Asks.Add(dto);
			}

			foreach (dynamic item in content.bids)
			{
				var dto = new OrderDto();
				dto.Price = item.price;
				dto.Amount = item.amount;
				model.Bids.Add(dto);
			}

			return model;
		}
	}
}