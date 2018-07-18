using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace Yobit.Api
{
	internal sealed class YobitApi
	{
		//internal async Task<HttpResponseMessage> GetOrderInfoAsync(int orderId, IYobitSettings settings)
		//{
		//	string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "OrderInfo" }, { "order_id", orderId.ToString() }, { "nonce", GenerateNonce(settings.CreatedAt) } }, true);
		//	GeneratePrivateHeaders(settings, queryString);
		//	HttpResponseMessage response = await HttpClient.PostAsync(PrivateUrl, new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));

		//	if (!response.IsSuccessStatusCode)
		//	{
		//		//throw new YobitException("Ocurred some error...");
		//	}

		//	return response;
		//}

		//internal async Task<HttpResponseMessage> GetTradesAsync(string pair, uint? limit = null)
		//{
		//	string queryString = limit.HasValue
		//		? $"trades/{pair}?limit={limit.Value}&ignore_invalid"
		//		: $"trades/{pair}";
		//	HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + queryString);

		//	if (!response.IsSuccessStatusCode)
		//	{
		//		//throw new YobitException("Ocurred some error...");
		//	}

		//	return response;
		//}

		//internal async Task<HttpResponseMessage> GetInfoAsync(IYobitSettings settings)
		//{
		//	string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "getInfo" }, { "nonce", GenerateNonce(settings.CreatedAt) } }, true);
		//	GeneratePrivateHeaders(settings, queryString);
		//	HttpResponseMessage response = await HttpClient.PostAsync(PrivateUrl, new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));

		//	if (!response.IsSuccessStatusCode)
		//	{
		//		//throw new YobitException("Ocurred some error...");
		//	}

		//	return response;
		//}

		//internal async Task<HttpResponseMessage> GetActiveOrdersOfUserAsync(IYobitSettings settings, string pair)
		//{
		//	string queryString = HttpHelper.QueryString(new Dictionary<string, string> { { "method", "ActiveOrders" }, { "pair", pair }, { "nonce", GenerateNonce(settings.CreatedAt) } }, true);
		//	GeneratePrivateHeaders(settings, queryString);
		//	HttpResponseMessage response = await HttpClient.PostAsync(PrivateUrl, new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"));

		//	if (!response.IsSuccessStatusCode)
		//	{
		//		//throw new YobitException("Ocurred some error...");
		//	}

		//	return response;
		//}
	}
}