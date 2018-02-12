
namespace Yobit.Api
{
	using Entities;
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Text;
	using System.Threading.Tasks;
    using TradingBot.Common;
    using TradingBot.Core;
    using TradingBot.Core.Enums;

	public class YobitPairsResponse : PairsResponse<Dictionary<string, Pair>>
	{
		public YobitPairsResponse(string error) : base(error)
		{
		}

		public YobitPairsResponse(Dictionary<string, Pair> data) : base(data)
		{
		}
	}

	public class YobitApi : ExchangeApi
	{
		public YobitApi(string baseAddress) : base(baseAddress)
		{
			Type = AccountType.Yobit;
		}

		public async Task<PairsInfo> GetPairsAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(new Uri(HttpClient.BaseAddress + "info"));

			return await AcquireContentAsync<PairsInfo>(response);
		}

		public PairsInfo GetPairs()
		{
			return GetPairsAsync().Result;
		}

		public override PairsResponse<T> GetPairs<T>()
		{
			var result = GetPairs();
			return result as PairsResponse<T>;
		}

		public async Task<PairData> GetPairDataAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			HttpResponseMessage response = await HttpClient.GetAsync(new Uri(String.Format(HttpClient.BaseAddress + "ticker/{0}", pair)));
			var result = await AcquireContentAsync<dynamic>(response);
			var model = new PairData
			{
				High = result[pair].high,
				Low = result[pair].low,
				Avg = result[pair].avg,
				Vol = result[pair].vol,
				VolCur = result[pair].vol_cur,
				Last = result[pair].last,
				Buy = result[pair].buy,
				Sell = result[pair].sell
			};
			
			return model;
		}

		public PairData GetPairData(string pair)
		{
			return GetPairDataAsync(pair).Result;
		}

		public async Task<PairOrders> GetPairOrdersAsync(string pair, uint? limit = null)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			string queryString = limit.HasValue
				? String.Format(HttpClient.BaseAddress + "depth/{0}?limit={1}", pair, limit.Value)
				: String.Format(HttpClient.BaseAddress + "depth/{0}", pair);

			HttpResponseMessage response = await HttpClient.GetAsync(new Uri(queryString));
			var result = await AcquireContentAsync<dynamic>(response);
			var model = new PairOrders();

			foreach (dynamic order in result[pair].asks)
			{
				model.Asks.Add(new Order { Price = order[0], Amount = order[1] });
			}

			foreach (dynamic order in result[pair].bids)
			{
				model.Bids.Add(new Order { Price = order[0], Amount = order[1] });
			}

			return model;
		}

		public PairOrders GetPairOrders(string pair, uint? limit = null)
		{
			return GetPairOrdersAsync(pair, limit).Result;
		}

		#region Private methods

		private async Task<TModel> AcquireContentAsync<TModel>(HttpResponseMessage message)
		{
			if (message.IsSuccessStatusCode)
			{
				byte[] buffer = await message.Content.ReadAsByteArrayAsync();
				string json = Encoding.Default.GetString(buffer);
				var result = JsonHelper.FromJson<TModel>(json);
				
				return result;
			}

			return default;
		}

		#endregion
	}
}