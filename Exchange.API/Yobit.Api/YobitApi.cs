
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
		private readonly Uri _privateUrl;
		private readonly Uri _publicUrl;
		private readonly IYobitSettings _settings;

		public YobitApi(IYobitSettings settings) : base(settings.BaseAddress)
		{
			if (settings == null)
			{
				throw new ArgumentNullException(nameof(settings), "The api settings are not provided.");
			}

			_privateUrl = new Uri(Client.BaseAddress + "tapi/");
			_publicUrl = new Uri(Client.BaseAddress + "api/3/");
			_settings = settings;
			Type = AccountType.Yobit;
		}

		public async Task<HttpResponseMessage> GetTradesAsync(string pair, uint? limit = null)
		{
			string queryString = limit.HasValue
				? String.Format("trades/{0}?limit={1}&ignore_invalid", pair, limit.Value)
				: String.Format("trades/{0}", pair);
			HttpResponseMessage response = await Client.GetAsync(_publicUrl + queryString);

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Ocurred some error...");
			}

			return response;
		}

		public async Task<HttpResponseMessage> GetInfoAsync()
		{
			int nonce = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
			var parameters = new Dictionary<string, string> { { "method", "getInfo" }, { "nonce", nonce.ToString() } };
			string queryString = HttpHelper.QueryString(parameters, true);
			var hash = new HashAlgorithm(_settings.Secret);
			string sign = BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(queryString)));
			Client.DefaultRequestHeaders.Add("Key", _settings.PublicKey);
			Client.DefaultRequestHeaders.Add("Sign", sign.Replace("-", "").ToLower());
			HttpResponseMessage response = await Client.PostAsync(_privateUrl, new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));

			return response;
		}

		public async Task<HttpResponseMessage> GetActiveOrdersOfUserAsync(string pair)
		{
			if (String.IsNullOrEmpty(_settings.PublicKey))
			{
				throw new ArgumentNullException(nameof(_settings.PublicKey));
			}

			if (String.IsNullOrEmpty(_settings.Secret))
			{
				throw new ArgumentNullException(nameof(_settings.Secret));
			}

			int nonce = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
			string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "ActiveOrders" }, { "pair", pair }, { "nonce", nonce.ToString() } }, true);
			var hash = new HashAlgorithm(_settings.Secret);
			string sign = BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(queryString)));
			Client.DefaultRequestHeaders.Add("Key", _settings.PublicKey);
			Client.DefaultRequestHeaders.Add("Sign", sign.Replace("-", "").ToLower());
			HttpResponseMessage response = await Client.PostAsync(_privateUrl, new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));

			return response;
		}

		public async Task<HttpResponseMessage> GetPairsAsync()
		{
			HttpResponseMessage response = await Client.GetAsync(new Uri(_publicUrl + "info?ignore_invalid=1"));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Occured some error...");
			}

			return response;
		}

		public async Task<HttpResponseMessage> GetPairDataAsync(string pair)
		{
			HttpResponseMessage response = await Client.GetAsync(new Uri(String.Format(_publicUrl + "ticker/{0}?ignore_invalid=1", pair)));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Occured some error...");
			}

			return response;
		}

		public async Task<HttpResponseMessage> GetPairOrdersAsync(string pair, uint? limit = null)
		{
			string queryString = limit.HasValue
				? String.Format(_publicUrl + "depth/{0}?limit={1}&ignore_invalid=1", pair, limit.Value)
				: String.Format(_publicUrl + "depth/{0}?ignore_invalid=1", pair);

			HttpResponseMessage response = await Client.GetAsync(new Uri(queryString));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Occured some error...");
			}

			return response;
		}
	}
}