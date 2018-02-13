
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
    using TradingBot.Data.Entities;

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
        private static string paramsTemplate = "?{0}nonce={1}";

        public YobitApi(string baseAddress) : base(baseAddress)
		{
			Type = AccountType.Yobit;
		}

		public async Task<object> GetActiveOrdersOfUserAsync(string pair, Account account)
		{
            if(account == null || account.YobitSettings == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			if (String.IsNullOrEmpty(account.ApiKey))
			{
				throw new ArgumentNullException("ApiKey");
			}

			if (String.IsNullOrEmpty(account.YobitSettings.Secret))
			{
				throw new ArgumentNullException("Secret");
			}

            var counter = account.YobitSettings.Counter;
            if (counter == 0)
                counter++;

            var h = new HashAlgorithm(account.YobitSettings.Secret);

            var parameters = string.Format("pair={0}&", pair);
            string postData = String.Format(paramsTemplate, parameters, counter);

			string sign = Convert.ToBase64String(h.ComputeHash(Encoding.Default.GetBytes(postData)));
			HttpClient.DefaultRequestHeaders.Add("Key", account.ApiKey);
			HttpClient.DefaultRequestHeaders.Add("Sign", sign);

            var httpContext = new StringContent(postData);
			var response = await HttpClient.PostAsync(new Uri(HttpClient.BaseAddress + "activeorders"), httpContext);

            var data = await HttpHelper.AcquireContentAsync<dynamic>(response);

            return data;
		}

		public object GetActiveOrdersOfUser(string pair, Account account)
		{
			return GetActiveOrdersOfUserAsync(pair, account).Result;
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