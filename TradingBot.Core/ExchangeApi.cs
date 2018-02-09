
namespace TradingBot.Core
{
	using System;
	using System.Net.Http;

	public abstract class ExchangeApi : IDisposable
	{
	    protected readonly HttpClient Http = new HttpClient();

	    protected ExchangeApi(string baseAddress)
	    {
		    Http.BaseAddress = new Uri(baseAddress);
		    Http.DefaultRequestHeaders.ConnectionClose = false;
	    }

	    protected virtual void Dispose(bool disposing)
	    {
		    if (disposing)
		    {
			    Http.Dispose();
		    }
	    }

	    public void Dispose()
	    {
		    Dispose(true);
		    GC.SuppressFinalize(this);
	    }
	}
}