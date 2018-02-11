
namespace TradingBot.Core
{
	using System;
	using System.Net.Http;

	public abstract class ExchangeApi : IDisposable
	{
	    protected readonly HttpClient HttpClient = new HttpClient();

	    protected ExchangeApi(string baseAddress)
	    {
		    HttpClient.BaseAddress = new Uri(baseAddress);
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