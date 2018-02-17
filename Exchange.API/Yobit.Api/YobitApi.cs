
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
		public YobitApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{
            Type = AccountType.Yobit;
		}

		public async Task<HttpResponseMessage> GetTradesAsync(string pair, uint? limit = null)
		{
			string queryString = limit.HasValue
				? String.Format("trades/{0}?limit={1}&ignore_invalid", pair, limit.Value)
				: String.Format("trades/{0}", pair);
			HttpResponseMessage response = await HttpClient.GetAsync(_publicUrl + queryString);

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Ocurred some error...");
			}

			return response;
		}

        private void GeneratePrivateHeaders(IYobitSettings settings, string queryString)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings), "The api settings are not provided.");
            }

            if (String.IsNullOrEmpty(settings.ApiKey))
            {
                throw new ArgumentNullException(nameof(settings.ApiKey));
            }

            if (String.IsNullOrEmpty(settings.Secret))
            {
                throw new ArgumentNullException(nameof(settings.Secret));
            }
           
            var hash = new HashAlgorithm(settings.Secret);
            string sign = BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(queryString)));

            if (HttpClient.DefaultRequestHeaders.Contains("Key"))
                HttpClient.DefaultRequestHeaders.Remove("Key");
            if (HttpClient.DefaultRequestHeaders.Contains("Sign"))
                HttpClient.DefaultRequestHeaders.Remove("Sign");

            HttpClient.DefaultRequestHeaders.Add("Key", settings.ApiKey);
            HttpClient.DefaultRequestHeaders.Add("Sign", sign.Replace("-", "").ToLower());
        }

		public async Task<HttpResponseMessage> GetInfoAsync(IYobitSettings settings)
        {
            int nonce = (int)(DateTime.UtcNow - new DateTime(2018, 1, 1)).TotalSeconds;
            string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "getInfo" }, { "nonce", nonce.ToString() } }, true);

            GeneratePrivateHeaders(settings, queryString);
            HttpResponseMessage response = await HttpClient.PostAsync(_privateUrl, new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));

			return response;
		}

		public async Task<HttpResponseMessage> GetActiveOrdersOfUserAsync(IYobitSettings settings, string pair)
		{
            int nonce = (int)(DateTime.UtcNow - settings.CreatedOn).TotalSeconds; //don't forget to change on: new DateTime(2018, 1, 1) 
            string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "ActiveOrders" }, { "pair", pair }, { "nonce", nonce.ToString() } }, true);

            GeneratePrivateHeaders(settings, queryString);
            
			HttpResponseMessage response = await HttpClient.PostAsync(_privateUrl, new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));

			return response;
		}

		public async Task<HttpResponseMessage> GetPairsAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(new Uri(_publicUrl + "info?ignore_invalid=1"));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Occured some error...");
			}

			return response;
		}

		public async Task<HttpResponseMessage> GetPairDataAsync(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(new Uri(String.Format(_publicUrl + "ticker/{0}?ignore_invalid=1", pair)));

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

			HttpResponseMessage response = await HttpClient.GetAsync(new Uri(queryString));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Occured some error...");
			}

			return response;
		}
	}
}