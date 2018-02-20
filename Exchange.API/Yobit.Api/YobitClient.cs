
namespace Yobit.Api
{
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;
	using TradingBot.Common;
	using TradingBot.Core;
	using TradingBot.Core.Entities;

	public sealed class YobitClient : IExchangeClient
	{
		private readonly YobitApi _api;
		private readonly IYobitSettings _settings;

		public YobitClient(string publicEndpoint, string privateEndpoint, IYobitSettings settings)
		{
			_settings = settings;
			_api = new YobitApi(publicEndpoint, privateEndpoint);
		}

		public async Task<OrderDetails> GetOrderInfoAsync(int orderId)
		{
			if (orderId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(orderId));
			}

			try
			{
				HttpResponseMessage response = await _api.GetOrderInfoAsync(orderId, _settings);
				var result = await HttpHelper.AcquireContentAsync<dynamic>(response);

				if (!result.success)
				{
					throw new YobitException(result.Error); //Hack because private API always returns 200 status code.
				}

				var model = new OrderDetails();

				foreach (dynamic item in result.@return)
				{
					dynamic value = item.Value;
					model.Orders.Add((int)item.Key, new OrderInfo
					{
						Pair = value.pair,
						Type = Enum.Parse(typeof(OrderType), value.type, true),
						StartAmount = value.start_amount,
						Amount = value.amount,
						Price = value.rate,
						CreatedAt = DateTimeOffset.FromUnixTimeSeconds(value.timestamp_created),
						Status = Enum.Parse(typeof(OrderStatus), value.status)
					});
				}

				return model;
			}
			catch (YobitException ex)
			{
				throw new HttpRequestException(ex.Message, ex);
			}
		}

		public OrderDetails GetOrderInfo(int orderId)
		{
			return GetOrderInfoAsync(orderId).Result;
		}

		public async Task<CancelOrder> CancelOrderAsync(int orderId)
		{
			if (orderId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(orderId));
			}

			try
			{
				HttpResponseMessage response = await _api.CancelTradeAsync(orderId, _settings);
				var result = await HttpHelper.AcquireContentAsync<dynamic>(response);

				if (!(bool)result.success)
				{
					throw new YobitException(result.Error); //Hack because private API always returns 200 status code.
				}

				var model = new CancelOrder();
				model.OrderId = result.@return.order_id;

				foreach (dynamic item in result.@return.funds)
				{
					model.Funds.Add(item.Key, item.Value);
				}

				return model;
			}
			catch (YobitException ex)
			{
				throw new HttpRequestException(ex.Message, ex);
			}
		}

		public CancelOrder CancelOrder(int orderId)
		{
			return CancelOrderAsync(orderId).Result;
		}

		public async Task<CreateOrder> CreateOrderAsync(string pair, OrderType type, decimal price, decimal amount)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			try
			{
				HttpResponseMessage response = await _api.CreateOrderAsync(pair, type, price, amount, _settings);
				var result = await HttpHelper.AcquireContentAsync<dynamic>(response);

				if (!(bool)result.success)
				{
					throw new YobitException(result.Error); //Hack because private API always returns 200 status code.
				}

				var model = new CreateOrder();
				model.Received = result.received;
				model.Remains = result.remains;
				model.OrderId = result.order_id;

				foreach (dynamic item in result.@return.funds)
				{
					model.Funds.Add(item.Key, item.Value);
				}

				return model;
			}
			catch (YobitException ex)
			{
				throw new HttpRequestException(ex.Message, ex);
			}
		}

		public CreateOrder CreateOrder(string pair, OrderType type, decimal price, decimal amount)
		{
			return CreateOrderAsync(pair, type, price, amount).Result;
		}

		public async Task<Balance> GetInfoAsync()
		{
			try
			{
				HttpResponseMessage response = await _api.GetInfoAsync(_settings);
				var result = await HttpHelper.AcquireContentAsync<dynamic>(response);

				if (!(bool)result.success)
				{
					throw new YobitException(result.Error); //Hack because private API always returns 200 status code.
				}

				var model = new Balance();

				foreach (dynamic item in result.@return.funds)
				{
					model.Funds.Add(item.Name, (decimal)item.Value);
				}

				foreach (dynamic item in result.@return.funds_incl_orders)
				{
					model.FundsIncludeOrders.Add(item.Name, (decimal)item.Value);
				}

				model.TransactionCount = result.@return.transaction_count;
				model.OpenOrders = result.@return.open_orders;

				return model;
			}
			catch (YobitException ex)
			{
				throw new HttpRequestException(ex.Message, ex);
			}
		}

		public Balance GetInfo()
		{
			return GetInfoAsync().Result;
		}

		public async Task<TradeInfo> GetTradesAsync(string pair, uint limit = 150)
		{
			try
			{
				HttpResponseMessage response = await _api.GetTradesAsync(pair, limit);
				var result = await HttpHelper.AcquireContentAsync<dynamic>(response);
				var model = new TradeInfo();

				foreach (dynamic item in result[pair])
				{
					var trade = new Trade
					{
						Type = (TradeType)Enum.Parse(typeof(TradeType), (string)item.type, true),
						Price = item.price,
						Amount = item.amount,
						Tid = item.tid,
						Timestamp = DateTimeOffset.FromUnixTimeSeconds((long)item.timestamp)
					};
					model.Trades.Add(trade);
				}

				return model;
			}
			catch (YobitException ex)
			{
				throw new HttpRequestException(ex.Message, ex);
			}
		}

		public TradeInfo GetTrades(string pair, uint limit = 150)
		{
			return GetTradesAsync(pair, limit).Result;
		}

		public async Task<PairsInfo> GetPairsAsync()
		{
			try
			{
				var info = await HttpHelper.AcquireContentAsync<PairsInfo>(await _api.GetPairsAsync());

				return info;
			}
			catch (YobitException ex)
			{
				throw new HttpRequestException(ex.Message, ex);
			}
		}

		public PairsInfo GetPairs()
		{
			return GetPairsAsync().Result;
		}

		public async Task<PairData> GetPairDataAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			try
			{
				var data = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairDataAsync(pair));
				var model = new PairData
				{
					High = data[pair].high,
					Low = data[pair].low,
					Avg = data[pair].avg,
					Vol = data[pair].vol,
					VolCur = data[pair].vol_cur,
					Last = data[pair].last,
					Buy = data[pair].buy,
					Sell = data[pair].sell
				};

				return model;
			}
			catch (YobitException ex)
			{
				throw new HttpRequestException(ex.Message, ex);
			}
		}

		public PairData GetPairData(string pair)
		{
			return GetPairDataAsync(pair).Result;
		}

		public async Task<PairOrders> GetPairOrdersAsync(string pair, uint limit = 150)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			try
			{
				var result = await HttpHelper.AcquireContentAsync<dynamic>(await _api.GetPairOrdersAsync(pair, limit));
				var model = new PairOrders();

				foreach (dynamic order in result[pair].asks)
				{
					model.Asks.Add(new Order { Rate = order[0], Amount = order[1] });
				}

				foreach (dynamic order in result[pair].bids)
				{
					model.Bids.Add(new Order { Rate = order[0], Amount = order[1] });
				}

				return model;
			}
			catch (YobitException ex)
			{
				throw new HttpRequestException(ex.Message, ex);
			}
		}

		public PairOrders GetPairOrders(string pair, uint limit = 150)
		{
			return GetPairOrdersAsync(pair, limit).Result;
		}

		public async Task<OrderDetails> GetActiveOrdersOfUserAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			try
			{
				HttpResponseMessage response = await _api.GetActiveOrdersOfUserAsync(_settings, pair);
				var result = await HttpHelper.AcquireContentAsync<dynamic>(response);

				if (!(bool)result.success)
				{
					throw new YobitException(result.Error); //Hack because private API always returns 200 status code.
				}

				var model = new OrderDetails();

				foreach (dynamic item in result.@return)
				{
					dynamic value = item.Value;
					model.Orders.Add((int)item.Key, new OrderInfo
					{
						Pair = value.pair,
						Type = Enum.Parse(typeof(OrderType), value.type, true),
						Amount = value.amount,
						Price = value.rate,
						CreatedAt = DateTimeOffset.FromUnixTimeSeconds(value.timestamp_created)
					});
				}

				return model;
			}
			catch (YobitException ex)
			{
				throw new HttpRequestException(ex.Message, ex);
			}
		}

		public OrderDetails GetActiveOrdersOfUser(string pair)
		{
			return GetActiveOrdersOfUserAsync(pair).Result;
		}
	}
}