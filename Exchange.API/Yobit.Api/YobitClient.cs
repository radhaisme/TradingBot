
namespace Yobit.Api
{
	using Entities;
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;
	using TradingBot.Common;
	using TradingBot.Core;

	public sealed class YobitClient : IExchangeClient
	{
		private readonly YobitApi _api;
		private IYobitSettings _settings;
		private int _counter;

		public YobitClient(IYobitSettings settings)
		{
			_counter = settings.Counter;
			_settings = settings;
			_api = new YobitApi(settings);
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
	}
}