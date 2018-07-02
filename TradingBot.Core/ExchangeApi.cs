using System;
using System.Net.Http;

namespace TradingBot.Core
{
	public abstract class ExchangeApi : IDisposable
	{
		protected readonly HttpClient HttpClient = new HttpClient();
		protected readonly Uri PrivateUrl;
		protected readonly Uri PublicUrl;

		protected ExchangeApi(string publicEndpoint, string privateEndpoint)
		{
			if (String.IsNullOrEmpty(publicEndpoint))
			{
				throw new ArgumentNullException(nameof(publicEndpoint));
			}

			if (String.IsNullOrEmpty(privateEndpoint))
			{
				throw new ArgumentNullException(nameof(privateEndpoint));
			}

			PrivateUrl = new Uri(NormalizeUrl(privateEndpoint));
			PublicUrl = new Uri(NormalizeUrl(publicEndpoint));
			HttpClient.DefaultRequestHeaders.ConnectionClose = false;
		}

		public void Dispose()
		{
			HttpClient.Dispose();
			GC.SuppressFinalize(this);
		}

		protected string NormalizeUrl(string url)
		{
			if (!url.EndsWith("/"))
			{
				return url + "/";
			}

			return url;
		}
	}
}