using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Yobit.Api
{
	public sealed class YobitClient : IExchangeClient
	{
		private readonly YobitApi _api;
		private readonly IYobitSettings _settings;

		public YobitClient()
		{
			_settings = new YobitSettings();
			_api = new YobitApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IReadOnlyCollection<Pair>> GetPairsAsync()
		{
			try
			{
				var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairsAsync());
				var pairs = new List<Pair>();

				foreach (dynamic item in content.pairs)
				{
					var pair = new Pair((string)item.Name);
					string[] assets = ((string)item.Name).Split('_');
					pair.BaseAsset = assets[0];
					pair.QuoteAsset = assets[1];
					pairs.Add(pair);
				}

				return new ReadOnlyCollection<Pair>(pairs);
			}
			catch (YobitException ex)
			{
				throw new HttpRequestException(ex.Message, ex);
			}
		}

		public async Task<PairDetail> GetPairDetailAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			try
			{
				var content = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairDetailAsync(pair));
				var detail = new PairDetail();
				detail.LastPrice = content[pair].last;
				detail.Volume = content[pair].vol;
				detail.Ask = content[pair].buy;
				detail.Bid = content[pair].sell;
				detail.High = content[pair].high;
				detail.Low = content[pair].low;

				return detail;
			}
			catch (YobitException ex)
			{
				throw new HttpRequestException(ex.Message, ex);
			}
		}

		public Task<IReadOnlyCollection<PairDetail>> GetPairsDetails(params string[] pairs)
		{
			throw new NotImplementedException();
		}

		//public async Task<OrderDetails> GetOrderInfoAsync(int orderId)
		//{
		//	if (orderId <= 0)
		//	{
		//		throw new ArgumentOutOfRangeException(nameof(orderId));
		//	}

		//	try
		//	{
		//		HttpResponseMessage response = await _api.GetOrderInfoAsync(orderId, _settings);
		//		var result = await HttpHelper.AcquireContentAsync<dynamic>(response);

		//		if (!result.success)
		//		{
		//			throw new YobitException(result.Error); //Hack because private API always returns 200 status code.
		//		}

		//		var model = new OrderDetails();

		//		foreach (dynamic item in result.@return)
		//		{
		//			dynamic value = item.Value;
		//			model.Orders.Add((int)item.Key, new OrderInfo
		//			{
		//				Pair = value.pair,
		//				Type = Enum.Parse(typeof(OrderType), value.type, true),
		//				StartAmount = value.start_amount,
		//				Amount = value.amount,
		//				Price = value.rate,
		//				CreatedAt = DateTimeOffset.FromUnixTimeSeconds(value.timestamp_created),
		//				Status = Enum.Parse(typeof(OrderStatus), value.status)
		//			});
		//		}

		//		return model;
		//	}
		//	catch (YobitException ex)
		//	{
		//		throw new HttpRequestException(ex.Message, ex);
		//	}
		//}

		//public async Task<CancelOrder> CancelOrderAsync(int orderId)
		//{
		//	if (orderId <= 0)
		//	{
		//		throw new ArgumentOutOfRangeException(nameof(orderId));
		//	}

		//	try
		//	{
		//		HttpResponseMessage response = await _api.CancelTradeAsync(orderId, _settings);
		//		var result = await HttpHelper.AcquireContentAsync<dynamic>(response);

		//		if (!(bool)result.success)
		//		{
		//			throw new YobitException(result.Error); //Hack because private API always returns 200 status code.
		//		}

		//		var model = new CancelOrder();
		//		model.OrderId = result.@return.order_id;

		//		foreach (dynamic item in result.@return.funds)
		//		{
		//			model.Funds.Add(item.Key, item.Value);
		//		}

		//		return model;
		//	}
		//	catch (YobitException ex)
		//	{
		//		throw new HttpRequestException(ex.Message, ex);
		//	}
		//}

		//public async Task<CreateOrder> CreateOrderAsync(string pair, OrderType type, decimal price, decimal amount)
		//{
		//	if (String.IsNullOrEmpty(pair))
		//	{
		//		throw new ArgumentNullException(nameof(pair));
		//	}

		//	try
		//	{
		//		HttpResponseMessage response = await _api.CreateOrderAsync(pair, type, price, amount, _settings);
		//		var result = await HttpHelper.AcquireContentAsync<dynamic>(response);

		//		if (!(bool)result.success)
		//		{
		//			throw new YobitException(result.Error); //Hack because private API always returns 200 status code.
		//		}

		//		var model = new CreateOrder();
		//		model.Received = result.received;
		//		model.Remains = result.remains;
		//		model.OrderId = result.order_id;

		//		foreach (dynamic item in result.@return.funds)
		//		{
		//			model.Funds.Add(item.Key, item.Value);
		//		}

		//		return model;
		//	}
		//	catch (YobitException ex)
		//	{
		//		throw new HttpRequestException(ex.Message, ex);
		//	}
		//}

		//public async Task<Balance> GetInfoAsync()
		//{
		//	try
		//	{
		//		HttpResponseMessage response = await _api.GetInfoAsync(_settings);
		//		var result = await HttpHelper.AcquireContentAsync<dynamic>(response);

		//		if (!(bool)result.success)
		//		{
		//			throw new YobitException(result.Error); //Hack because private API always returns 200 status code.
		//		}

		//		var model = new Balance();

		//		foreach (dynamic item in result.@return.funds)
		//		{
		//			model.Funds.Add(item.Name, (decimal)item.Value);
		//		}

		//		foreach (dynamic item in result.@return.funds_incl_orders)
		//		{
		//			model.FundsIncludeOrders.Add(item.Name, (decimal)item.Value);
		//		}

		//		model.TransactionCount = result.@return.transaction_count;
		//		model.OpenOrders = result.@return.open_orders;

		//		return model;
		//	}
		//	catch (YobitException ex)
		//	{
		//		throw new HttpRequestException(ex.Message, ex);
		//	}
		//}

		//public async Task<TradeInfo> GetTradesAsync(string pair, uint limit = 150)
		//{
		//	try
		//	{
		//		HttpResponseMessage response = await _api.GetTradesAsync(pair, limit);
		//		var result = await HttpHelper.AcquireContentAsync<dynamic>(response);
		//		var model = new TradeInfo();

		//		foreach (dynamic item in result[pair])
		//		{
		//			var trade = new Trade
		//			{
		//				Type = (TradeType)Enum.Parse(typeof(TradeType), (string)item.type, true),
		//				Price = item.price,
		//				Amount = item.amount,
		//				Tid = item.tid,
		//				Timestamp = DateTimeOffset.FromUnixTimeSeconds((long)item.timestamp)
		//			};
		//			model.Trades.Add(trade);
		//		}

		//		return model;
		//	}
		//	catch (YobitException ex)
		//	{
		//		throw new HttpRequestException(ex.Message, ex);
		//	}
		//}

		//public async Task<PairOrders> GetPairOrdersAsync(string pair, uint limit = 150)
		//{
		//	if (String.IsNullOrEmpty(pair))
		//	{
		//		throw new ArgumentNullException(nameof(pair));
		//	}

		//	try
		//	{
		//		var result = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairOrdersAsync(pair, limit));
		//		var model = new PairOrders();

		//		foreach (dynamic order in result[pair].asks)
		//		{
		//			model.Asks.Add(new Order { Rate = order[0], Amount = order[1] });
		//		}

		//		foreach (dynamic order in result[pair].bids)
		//		{
		//			model.Bids.Add(new Order { Rate = order[0], Amount = order[1] });
		//		}

		//		return model;
		//	}
		//	catch (YobitException ex)
		//	{
		//		throw new HttpRequestException(ex.Message, ex);
		//	}
		//}

		//public async Task<OrderDetails> GetActiveOrdersOfUserAsync(string pair)
		//{
		//	if (String.IsNullOrEmpty(pair))
		//	{
		//		throw new ArgumentNullException(nameof(pair));
		//	}

		//	try
		//	{
		//		HttpResponseMessage response = await _api.GetActiveOrdersOfUserAsync(_settings, pair);
		//		var result = await HttpHelper.AcquireContentAsync<dynamic>(response);

		//		if (!(bool)result.success)
		//		{
		//			throw new YobitException(result.Error); //Hack because private API always returns 200 status code.
		//		}

		//		var model = new OrderDetails();

		//		foreach (dynamic item in result.@return)
		//		{
		//			dynamic value = item.Value;
		//			model.Orders.Add((int)item.Key, new OrderInfo
		//			{
		//				Pair = value.pair,
		//				Type = Enum.Parse(typeof(OrderType), value.type, true),
		//				Amount = value.amount,
		//				Price = value.rate,
		//				CreatedAt = DateTimeOffset.FromUnixTimeSeconds(value.timestamp_created)
		//			});
		//		}

		//		return model;
		//	}
		//	catch (YobitException ex)
		//	{
		//		throw new HttpRequestException(ex.Message, ex);
		//	}
		//}
	}
}