
namespace Yobit.Api
{
	using Entities;
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Security.Cryptography;
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
		private readonly int _requestCount;

		public YobitApi(string baseAddress) : base(baseAddress)
		{
			Type = AccountType.Yobit;
			_requestCount = 0;
		}

		public async Task<object> GetActiveOrdersOfUserAsync(string pair, string key, string secret)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			if (String.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (String.IsNullOrEmpty(secret))
			{
				throw new ArgumentNullException(nameof(secret));
			}

			var h = new HashAlgorithm(secret);
			string q = String.Format("?pair={0}&nonce={1}", pair, _requestCount);
			string sign = Convert.ToBase64String(h.ComputeHash(Encoding.Default.GetBytes(q)));
			HttpClient.DefaultRequestHeaders.Add("Key", key);
			HttpClient.DefaultRequestHeaders.Add("Sign", sign);
			HttpResponseMessage response = await HttpClient.PostAsync(new Uri(HttpClient.BaseAddress + "activeorders" + q), null);



			return null;
		}

		public object GetActiveOrdersOfUser(string pair, string key, string secret)
		{
			return GetActiveOrdersOfUserAsync(pair, key, secret).Result;
		}

        public async Task<YobitPairsResponse> GetPairsAsync()
        {
            YobitPairsResponse result;
            var response = await HttpClient.GetAsync(new Uri(HttpClient.BaseAddress + "info"));

            try
            {
                var data = await HttpHelper.AcquireContentAsync<PairsInfo>(response);
                if (data == null)
                    result = new YobitPairsResponse("GetPairs response is not success");
                else
                    result = new YobitPairsResponse(data.Pairs);
            }
            catch (Exception ex)
            {
                result = new YobitPairsResponse(string.Format("Can't parse GetPairs response: {0}", ex.Message));
            }

            return result;
        }

		public YobitPairsResponse GetPairs()
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
			var result = await HttpHelper.AcquireContentAsync<dynamic>(response);
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
			var result = await HttpHelper.AcquireContentAsync<dynamic>(response);
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

		public PairOrders GetPairOrders(string pair, uint? limit = null)
		{
			return GetPairOrdersAsync(pair, limit).Result;
		}
	}
}