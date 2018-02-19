
namespace TradingBot.Core
{
	using Enums;
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;

	public class PairsResponse<T>
	{
		public bool IsSuccess { get; set; }
		public string Error { get; set; }
		public T Content { get; set; }

		public PairsResponse(T content)
		{
			IsSuccess = true;
			Content = content;
		}

		public PairsResponse(string error)
		{
			IsSuccess = true;
			Error = error;
		}
	}

	public class ApiResponse<T>
	{
		public bool IsSuccess { get; private set; }
		public T Content { get; set; }

		public ApiResponse()
		{
			IsSuccess = true;
		}
	}

	public class BasePairsResponse : PairsResponse<object>
	{
		public BasePairsResponse(object content) : base(content)
		{
		}
	}

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