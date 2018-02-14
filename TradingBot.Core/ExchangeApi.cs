
namespace TradingBot.Core
{
	using Enums;
	using System;
	using System.Net.Http;

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
		public AccountType Type { get; protected set; }
		protected readonly HttpClient Client = new HttpClient();

		protected ExchangeApi(string baseAddress)
		{
			Client.BaseAddress = new Uri(baseAddress);
			Client.DefaultRequestHeaders.ConnectionClose = false;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Client.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		//public abstract PairsResponse<T> GetPairs<T>();
	}
}