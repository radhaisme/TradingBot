using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;

namespace TradingBot.Core
{
	public abstract class ApiClient
	{
		private readonly HttpClient _client = new HttpClient();

		protected ApiClient()
		{
			_client.DefaultRequestHeaders.ConnectionClose = false;
		}

		public void Dispose()
		{
			_client.Dispose();
			GC.SuppressFinalize(this);
		}

		#region Private methods

		protected async Task<TContent> CallAsync<TContent>(HttpMethod method, Uri uri, HttpContent content = null)
		{
			var request = new HttpRequestMessage(method, uri) { Content = content };
			HttpResponseMessage response = await _client.SendAsync(request);
			HandleError(response);

			return await HttpHelper.AcquireContentAsync<TContent>(response);
		}

		protected virtual void HandleError(HttpResponseMessage response)
		{
			response.EnsureSuccessStatusCode();
		}

		protected Uri BuildUrl(string baseUrl, string url)
		{
			if (String.IsNullOrEmpty(baseUrl))
			{
				throw new ArgumentNullException(nameof(baseUrl));
			}

			if (String.IsNullOrEmpty(url))
			{
				throw new ArgumentNullException(nameof(url));
			}

			var current = baseUrl;

			if (!current.EndsWith("/"))
			{
				current = current + "/";
			}

			return new Uri(current + url);
		}

		protected void SetHeaders(IDictionary<string, string> headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException(nameof(headers));
			}

			foreach (KeyValuePair<string, string> header in headers)
			{
				if (_client.DefaultRequestHeaders.Contains(header.Key))
				{
					_client.DefaultRequestHeaders.Remove(header.Key);
				}

				_client.DefaultRequestHeaders.Add(header.Key, header.Value);
			}
		}

		#endregion
	}
}