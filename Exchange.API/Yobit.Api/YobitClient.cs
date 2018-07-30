using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace Yobit.Api
{
	public sealed class YobitClient : ApiClient, IApiClient
	{
		private readonly IYobitSettings _settings;

		public YobitClient()
		{
			_settings = new YobitSettings();
		}

		public ExchangeType Type => _settings.Type;

		public async Task<IReadOnlyCollection<PairDto>> GetPairsAsync()
		{
			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, "info?ignore_invalid=1"));
			var pairs = new List<PairDto>();

			foreach (dynamic key in content.pairs)
			{
				dynamic pair = content.pairs[(string)key.Name];

				if ((bool)pair.hidden)
				{
					continue;
				}

				var dto = new PairDto();
				string[] assets = ((string)key.Name).Split('_');
				dto.BaseAsset = assets[0];
				dto.QuoteAsset = assets[1];
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

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"ticker/{pair}?ignore_invalid=1"));
			dynamic data = content[pair];
			var dto = new PairDetailDto();
			dto.LastPrice = data.last;
			dto.Volume = data.vol;
			dto.Ask = data.buy;
			dto.Bid = data.sell;
			dto.High = data.high;
			dto.Low = data.low;

			return dto;
		}

		public async Task<DepthDto> GetOrderBookAsync(string pair, uint limit = 100)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			var content = await CallAsync<dynamic>(HttpMethod.Get, BuildUrl(_settings.PublicUrl, $"depth/{pair}?limit={limit}&ignore_invalid=1"));
			dynamic data = content[pair];
			DepthDto dto = Helper.BuildOrderBook(((IEnumerable<dynamic>)data.asks).Take((int)limit), ((IEnumerable<dynamic>)data.bids).Take((int)limit), item => new BookOrderDto { Price = item[0], Amount = item[1] });

			return dto;
		}

		public async Task<CreateOrderDto> CreateOrderAsync(OrderDto input)
		{
			if (String.IsNullOrEmpty(input.Pair))
			{
				throw new ArgumentNullException(nameof(input.Pair));
			}

			string queryString = HttpHelper.QueryString(new Dictionary<string, string>
			{
				{"method", "Trade"},
				{"pair", input.Pair},
				{"type", input.Side.ToString()},
				{"rate", input.Price.ToString(CultureInfo.InvariantCulture)},
				{"amount", input.Amount.ToString(CultureInfo.InvariantCulture)},
				{"nonce", GenerateNonce(_settings.CreatedAt)}
			}, true);
			var content = await MakePrivateCallAsync(queryString);
			var dto = new CreateOrderDto();
			dto.Received = content.received;
			dto.Remains = content.remains;
			dto.OrderId = content.order_id;

			return dto;
		}

		public async Task<CancelOrderDto> CancelOrderAsync(CancelOrderDto input)
		{
			if (input.OrderId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(input.OrderId));
			}

			string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "CancelOrderDto" }, { "order_id", input.OrderId.ToString() }, { "nonce", GenerateNonce(_settings.CreatedAt) } }, true);
			var content = await MakePrivateCallAsync(queryString);
			var dto = new CancelOrderDto();
			dto.OrderId = content.@return.order_id;

			return dto;
		}

		#region Private method

		private Task<dynamic> MakePrivateCallAsync(string content)
		{
			if (String.IsNullOrEmpty(_settings.PrivateUrl))
			{
				throw new ArgumentNullException(nameof(_settings.PrivateUrl));
			}

			if (String.IsNullOrEmpty(_settings.ApiKey))
			{
				throw new ArgumentNullException(nameof(_settings.ApiKey));
			}

			if (String.IsNullOrEmpty(_settings.Secret))
			{
				throw new ArgumentNullException(nameof(_settings.Secret));
			}

			SetHeaders(new Dictionary<string, string>
			{
				{"Key", _settings.ApiKey},
				{
					"Sign", BitConverter.ToString(new HMACSHA512(Encoding.UTF8.GetBytes(_settings.Secret)).ComputeHash(Encoding.UTF8.GetBytes(content))).Replace("-", "").ToLower()
				}
			});

			return CallAsync<dynamic>(HttpMethod.Post, BuildUrl(_settings.PrivateUrl, content), new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));
		}

		private string GenerateNonce(DateTimeOffset date)
		{
			return (DateTime.UtcNow - date).TotalSeconds.ToString(CultureInfo.InvariantCulture);
		}

		protected override async void HandleError(HttpResponseMessage response)
		{
			var content = await HttpHelper.AcquireContentAsync<ErrorModel>(response);

			if (!content.Success && !String.IsNullOrEmpty(content.Error))
			{
				throw new HttpRequestException(content.Error);
			}
		}

		#endregion

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
		//				Type = Enum.Parse(typeof(Side), value.type, true),
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
		//				Type = Enum.Parse(typeof(Side), value.type, true),
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