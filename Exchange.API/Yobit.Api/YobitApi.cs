
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

	public sealed class YobitApi : ExchangeApi
	{
		private static string paramsTemplate = "?method={0}&{1}nonce={2}";
		private readonly IYobitSettings _settings;

		public YobitApi(IYobitSettings settings) : base(settings.BaseAddress)
		{
			if (settings == null)
			{
				throw new ArgumentNullException(nameof(settings), String.Format("The api settings are not provided."));
			}

			_settings = settings;
			Type = AccountType.Yobit;
		}

		public async Task<dynamic> GetActiveOrdersOfUserAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			if (String.IsNullOrEmpty(_settings.PublicKey))
			{
				throw new ArgumentNullException(nameof(_settings.PublicKey));
			}

			if (String.IsNullOrEmpty(_settings.Secret))
			{
				throw new ArgumentNullException(nameof(_settings.Secret));
			}

			//var counter = account.YobitSettings.Counter;

			//if (counter == 0)
			//{
			//	counter++;
			//}

			//var h = new HashAlgorithm(_settings.Secret);

			//var parameters = string.Format("pair={0}&", pair);
			//string postData = String.Format(paramsTemplate, "activeOrders", parameters, counter);

			//string sign = Convert.ToBase64String(h.ComputeHash(Encoding.Default.GetBytes(postData)));
			//Client.DefaultRequestHeaders.Add("Key", _settings.PublicKey);
			//Client.DefaultRequestHeaders.Add("Sign", sign);

			//var url = new Uri(PrivateEndpoint);
			//var content = new StringContent(postData);
			//var response = await Client.PostAsync(url, content);

			//var data = await HttpHelper.AcquireContentAsync<dynamic>(response);

			return null;
		}

		public dynamic GetActiveOrdersOfUser(string pair)
		{
			return GetActiveOrdersOfUserAsync(pair).Result;
		}

		public async Task<YobitPairsResponse> GetPairsAsync()
		{
			YobitPairsResponse result;
			var response = await Client.GetAsync(new Uri(Client.BaseAddress + "info"));

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

			HttpResponseMessage response = await Client.GetAsync(new Uri(String.Format(Client.BaseAddress + "ticker/{0}", pair)));
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
				? String.Format(Client.BaseAddress + "depth/{0}?limit={1}", pair, limit.Value)
				: String.Format(Client.BaseAddress + "depth/{0}", pair);

			HttpResponseMessage response = await Client.GetAsync(new Uri(queryString));
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