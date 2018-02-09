
namespace Yobit.Exchange.Api
{
	using System;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using System.Threading.Tasks;

	public class YobitClient
    {
	    public string GetPairData(string pair)
	    {
			var client = new HttpClient();
			
			//client.BaseAddress = new Uri("https://yobit.net/api/3/");
			//client.DefaultRequestHeaders.Clear();
			
		    HttpResponseMessage result = client.GetAsync(new Uri(String.Format("https://yobit.net/api/3/ticker/{0}", pair))).Result;
			//result.Content.Headers.ContentEncoding.Add("gzip");
		    var r = result.Content.ReadAsByteArrayAsync().Result;
		    string s = Encoding.Default.GetString(r);

			return s;
	    }
    }
}