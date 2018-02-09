
namespace Yobit.Exchange.Api
{
	using System;
	using System.Net.Http;
	using System.Text;
	using TradingBot.Core;

	public class YobitApi : ExchangeApi
	{		
	    public YobitApi(string baseAddress) : base(baseAddress)
	    { }

		public string GetPairs()
		{
			HttpResponseMessage response = Http.GetAsync(new Uri(Http.BaseAddress + "api/3/info")).Result;

			if (response.IsSuccessStatusCode)
			{
				var buffer = response.Content.ReadAsByteArrayAsync().Result;

				return Encoding.Default.GetString(buffer);
			}

			return null;
		}

		public string GetPairData(string pair)
	    {
		    if (String.IsNullOrEmpty(pair))
		    {
				throw new ArgumentNullException(nameof(pair));
		    }

			HttpResponseMessage response = Http.GetAsync(new Uri(String.Format(Http.BaseAddress + "api/3/ticker/{0}", pair))).Result;

		    if (response.IsSuccessStatusCode)
		    {
			    var buffer = response.Content.ReadAsByteArrayAsync().Result;
			    
				return Encoding.Default.GetString(buffer);
			}

			return null;
	    }
    }
}