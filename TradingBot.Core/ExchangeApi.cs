
namespace TradingBot.Core
{
	using System;
	using System.Net.Http;

	public abstract class ExchangeApi : IDisposable
	{
		protected readonly HttpClient HttpClient = new HttpClient();
		protected readonly Uri PrivateUrl;
		protected readonly Uri PublicUrl;

		protected ExchangeApi(string publicEndpoint, string privateEndpoint)
		{
			PrivateUrl = new Uri(privateEndpoint);
			PublicUrl = new Uri(publicEndpoint);
			HttpClient.DefaultRequestHeaders.ConnectionClose = false;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				HttpClient.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}