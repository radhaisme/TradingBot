
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

		public async Task<HttpResponseMessage> GetActiveOrdersOfUserAsync(string pair, int counter)
		{
			if (String.IsNullOrEmpty(_settings.PublicKey))
			{
				throw new ArgumentNullException(nameof(_settings.PublicKey));
			}

			if (String.IsNullOrEmpty(_settings.Secret))
			{
				throw new ArgumentNullException(nameof(_settings.Secret));
			}

			string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "ActiveOrders" }, { "pair", pair }, { "nonce", counter.ToString() } });
			var hash = new HashAlgorithm(_settings.Secret);
			string sign = Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(queryString)));
			Client.DefaultRequestHeaders.Add("Key", _settings.PublicKey);
			Client.DefaultRequestHeaders.Add("Sign", sign);
			HttpResponseMessage response = await Client.PostAsync(new Uri(Client.BaseAddress + "tapi/"), new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));
			
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