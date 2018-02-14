
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

		public YobitPairsResponse(Dictionary<string, Pair> content) : base(content)
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
				throw new ArgumentNullException(nameof(settings), "The api settings are not provided.");
			}

			_settings = settings;
			Type = AccountType.Yobit;
		}

		public async Task<dynamic> GetActiveOrdersOfUserAsync(string pair, int counter)
		{
			if (String.IsNullOrEmpty(_settings.PublicKey))
			{
				throw new ArgumentNullException(nameof(_settings.PublicKey));
			}

			if (String.IsNullOrEmpty(_settings.Secret))
			{
				throw new ArgumentNullException(nameof(_settings.Secret));
			}

			var hash = new HashAlgorithm(_settings.Secret);
			var parameters = String.Format("pair={0}&", pair);
			string postData = String.Format(paramsTemplate, "activeOrders", parameters, counter);
			string sign = Convert.ToBase64String(hash.ComputeHash(Encoding.Default.GetBytes(postData)));
			Client.DefaultRequestHeaders.Add("Key", _settings.PublicKey);
			Client.DefaultRequestHeaders.Add("Sign", sign);

			var url = new Uri(String.Empty);
			var content = new StringContent(postData);
			HttpResponseMessage response = await Client.PostAsync(url, content);

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException(true, await HttpHelper.AcquireStringAsync(response));
			}

			return response;
		}

		public async Task<HttpResponseMessage> GetPairsAsync()
		{
			HttpResponseMessage response = await Client.GetAsync(new Uri(Client.BaseAddress + "api/3/info?ignore_invalid=1"));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Occurs some error...");
			}

			return response;
		}

		public async Task<HttpResponseMessage> GetPairDataAsync(string pair)
		{
			HttpResponseMessage response = await Client.GetAsync(new Uri(String.Format(Client.BaseAddress + "api/3/ticker/{0}?ignore_invalid=1", pair)));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Occurs some error...");
			}

			return response;
		}

		public async Task<HttpResponseMessage> GetPairOrdersAsync(string pair, uint? limit = null)
		{
			string queryString = limit.HasValue
				? String.Format(Client.BaseAddress + "api/3/depth/{0}?limit={1}&ignore_invalid=1", pair, limit.Value)
				: String.Format(Client.BaseAddress + "api/3/depth/{0}?ignore_invalid=1", pair);

			HttpResponseMessage response = await Client.GetAsync(new Uri(queryString));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Occurs some error...");
			}

			return response;
		}
	}
}