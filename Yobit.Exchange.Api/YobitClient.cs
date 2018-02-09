
namespace Yobit.Exchange.Api
{
	using System;
	using System.Net.Http;
	using System.Text;

	public class YobitClient : IDisposable
    {
		private readonly HttpClient _http = new HttpClient();

	    public YobitClient()
	    {
		    _http.DefaultRequestHeaders.ConnectionClose = false;
	    }

		public string GetPairData(string pair)
	    {
		    if (String.IsNullOrEmpty(pair))
		    {
				throw new ArgumentNullException(nameof(pair));
		    }

			HttpResponseMessage response = _http.GetAsync(new Uri(String.Format("https://yobit.net/api/3/ticker/{0}", pair))).Result;

		    if (response.IsSuccessStatusCode)
		    {
			    var buffer = response.Content.ReadAsByteArrayAsync().Result;
			    
				return Encoding.Default.GetString(buffer);
			}

			return null;
	    }

	    protected virtual void Dispose(bool disposing)
	    {
		    if (disposing)
		    {
			    _http?.Dispose();
		    }
	    }

	    public void Dispose()
	    {
		    Dispose(true);
		    GC.SuppressFinalize(this);
	    }
    }
}