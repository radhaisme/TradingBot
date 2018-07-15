using System;
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
			//if (String.IsNullOrEmpty(publicEndpoint))
			//{
			//	throw new ArgumentNullException(nameof(publicEndpoint));
			//}

			//if (String.IsNullOrEmpty(privateEndpoint))
			//{
			//	throw new ArgumentNullException(nameof(privateEndpoint));
			//}

			//PrivateUrl = new Uri(NormalizeUrl(privateEndpoint));
			//PublicUrl = new Uri(NormalizeUrl(publicEndpoint));

			_client.DefaultRequestHeaders.ConnectionClose = false;
		}

		protected async Task<TContent> CallAsync<TContent>(HttpMethod method, Uri uri, HttpContent content = null)
		{
			var request = new HttpRequestMessage(method, uri);
			request.Content = content;
			HttpResponseMessage response = await _client.SendAsync(request);

			if (!response.IsSuccessStatusCode)
			{
				HandleError(response);
			}

			return await HttpHelper.AcquireContentAsync<TContent>(response);
		}

		protected abstract void HandleError(HttpResponseMessage response);

		public void Dispose()
		{
			_client.Dispose();
			GC.SuppressFinalize(this);
		}

		protected Uri BuildUrl(string baseUrl, string url)
		{
			var current = baseUrl;

			if (!current.EndsWith("/"))
			{
				current = current + "/";
			}

			return new Uri(current + url);
		}
	}
}