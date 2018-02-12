
namespace Yobit.Api
{
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Text;
	using System.Threading.Tasks;
	using Entities;
	using TradingBot.Core;
	using TradingBot.Data.Enums;

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

		public async Task<YobitPairsResponse> GetPairsAsync()
		{
			YobitPairsResponse result;
			HttpResponseMessage response = await HttpClient.GetAsync(new Uri(HttpClient.BaseAddress + "info"));

			if (response.IsSuccessStatusCode)
			{
				byte[] buffer = await response.Content.ReadAsByteArrayAsync();

				var json = Encoding.Default.GetString(buffer);
				try
				{
					dynamic data = JsonHelper.ToObject(json);

					var dict = new Dictionary<string, Pair>();
					foreach (var pair in data.pairs)
					{
						var name = pair.Name;
						Pair pairInfo = JsonHelper.ToObject<Pair>(pair.Value.ToString());
						dict.Add(name, pairInfo);
					}
					result = new YobitPairsResponse(dict);
				}
				catch (Exception ex)
				{
					result = new YobitPairsResponse(string.Format("Can't parse GetPairs response: {0}", ex.Message));
				}
			}
			else
				result = new YobitPairsResponse("GetPairs response is not success");

			return result;
		}

		public YobitPairsResponse GetPairs()
		{
			return GetPairsAsync().Result;
		}

		public override PairsResponse<T> GetPairs<T>()
		{
			var result = GetPairsAsync().Result;
			return result as PairsResponse<T>;
		}

		public async Task<string> GetPairDataAsync(string pair)
		{
			if (String.IsNullOrEmpty(pair))
			{
				throw new ArgumentNullException(nameof(pair));
			}

			HttpResponseMessage response = await HttpClient.GetAsync(new Uri(String.Format(HttpClient.BaseAddress + "ticker/{0}", pair)));

			if (response.IsSuccessStatusCode)
			{
				byte[] buffer = await response.Content.ReadAsByteArrayAsync();

				return Encoding.Default.GetString(buffer);
			}

			return null;
		}

		public string GetPairData(string pair)
		{
			return GetPairDataAsync(pair).Result;
		}
	}
}