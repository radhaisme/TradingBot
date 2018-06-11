
namespace Yobit.Api
{
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading.Tasks;
	using TradingBot.Common;
	using TradingBot.Core;
	using TradingBot.Core.Entities;

	internal sealed class YobitApi : ExchangeApi
	{
		public YobitApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
		{ }

		internal async Task<HttpResponseMessage> GetOrderInfoAsync(int orderId, IYobitSettings settings)
		{
			string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "OrderInfo" }, { "order_id", orderId.ToString() }, { "nonce", GenerateNonce(settings.CreatedAt) } }, true);
			GeneratePrivateHeaders(settings, queryString);
			HttpResponseMessage response = await HttpClient.PostAsync(PrivateUrl, new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Ocurred some error...");
			}

			return response;
		}

		internal async Task<HttpResponseMessage> GetTradesAsync(string pair, uint? limit = null)
		{
			string queryString = limit.HasValue
				? $"trades/{pair}?limit={limit.Value}&ignore_invalid"
				: $"trades/{pair}";
			HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + queryString);

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Ocurred some error...");
			}

			return response;
		}

		internal async Task<HttpResponseMessage> CancelTradeAsync(int orderId, IYobitSettings settings)
		{
			string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "CancelOrder" }, { "order_id", orderId.ToString() }, { "nonce", GenerateNonce(settings.CreatedAt) } }, true);
			GeneratePrivateHeaders(settings, queryString);
			HttpResponseMessage response = await HttpClient.PostAsync(PrivateUrl, new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Ocurred some error...");
			}

			return response;
		}

		internal async Task<HttpResponseMessage> CreateOrderAsync(string pair, OrderType type, decimal price, decimal amount, IYobitSettings settings)
		{
			string queryString = HttpHelper.QueryString(new Dictionary<string, string>
			{
				{"method", "Trade"},
				{"pair", pair},
				{"type", OrderType.Buy.ToString()},
				{"rate", price.ToString()},
				{"amount", amount.ToString()},
				{"nonce", GenerateNonce(settings.CreatedAt)}
			}, true);
			GeneratePrivateHeaders(settings, queryString);
			HttpResponseMessage response = await HttpClient.PostAsync(PrivateUrl, new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Ocurred some error...");
			}

			return response;
		}

		internal async Task<HttpResponseMessage> GetInfoAsync(IYobitSettings settings)
		{
			string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "getInfo" }, { "nonce", GenerateNonce(settings.CreatedAt) } }, true);
			GeneratePrivateHeaders(settings, queryString);
			HttpResponseMessage response = await HttpClient.PostAsync(PrivateUrl, new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Ocurred some error...");
			}

			return response;
		}

		internal async Task<HttpResponseMessage> GetActiveOrdersOfUserAsync(IYobitSettings settings, string pair)
		{
			string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "ActiveOrders" }, { "pair", pair }, { "nonce", GenerateNonce(settings.CreatedAt) } }, true);
			GeneratePrivateHeaders(settings, queryString);
			HttpResponseMessage response = await HttpClient.PostAsync(PrivateUrl, new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Ocurred some error...");
			}

			return response;
		}

		internal async Task<HttpResponseMessage> GetPairsAsync()
		{
			HttpResponseMessage response = await HttpClient.GetAsync(new Uri(PublicUrl + "info?ignore_invalid=1"));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Occured some error...");
			}

			return response;
		}

		internal async Task<HttpResponseMessage> GetPairDataAsync(string pair)
		{
			HttpResponseMessage response = await HttpClient.GetAsync(new Uri(String.Format(PublicUrl + "ticker/{0}?ignore_invalid=1", pair)));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Occured some error...");
			}

			return response;
		}

		internal async Task<HttpResponseMessage> GetPairOrdersAsync(string pair, uint? limit = null)
		{
			string queryString = limit.HasValue
				? String.Format(PublicUrl + "depth/{0}?limit={1}&ignore_invalid=1", pair, limit.Value)
				: String.Format(PublicUrl + "depth/{0}?ignore_invalid=1", pair);

			HttpResponseMessage response = await HttpClient.GetAsync(new Uri(queryString));

			if (!response.IsSuccessStatusCode)
			{
				throw new YobitException("Occured some error...");
			}

			return response;
		}

		#region Private methods

		private string GenerateNonce(DateTimeOffset date)
		{
			return (DateTime.UtcNow - date).TotalSeconds.ToString();
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

			var hash = new HMACSHA512(Encoding.UTF8.GetBytes(settings.Secret));
			string sign = BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(queryString)));

			if (HttpClient.DefaultRequestHeaders.Contains("Key"))
			{
				HttpClient.DefaultRequestHeaders.Remove("Key");
			}

			if (HttpClient.DefaultRequestHeaders.Contains("Sign"))
			{
				HttpClient.DefaultRequestHeaders.Remove("Sign");
			}

			HttpClient.DefaultRequestHeaders.Add("Key", settings.ApiKey);
			HttpClient.DefaultRequestHeaders.Add("Sign", sign.Replace("-", "").ToLower());
		}

		#endregion
	}
}