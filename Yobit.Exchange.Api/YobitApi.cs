
namespace Yobit.Exchange.Api
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using TradingBot.Core;

    public class YobitApi : ExchangeApi
	{		
	    public YobitApi(string baseAddress) : base(baseAddress)
	    { }

	    public async Task<string> GetPairsAsync()
	    {
            HttpResponseMessage response = await Http.GetAsync(new Uri(Http.BaseAddress + "api/3/info"));

            if (response.IsSuccessStatusCode)
            {
                byte[] buffer = await response.Content.ReadAsByteArrayAsync();

                return Encoding.Default.GetString(buffer);
            }

            return null;
	    }

        public string GetPairs()
		{
			return GetPairsAsync().Result;
		}

	    public async Task<string> GetPairDataAsync(string pair)
	    {
	        if (String.IsNullOrEmpty(pair))
	        {
	            throw new ArgumentNullException(nameof(pair));
	        }

	        HttpResponseMessage response = await Http.GetAsync(new Uri(String.Format(Http.BaseAddress + "api/3/ticker/{0}", pair)));

	        if (response.IsSuccessStatusCode)
	        {
	            byte[] buffer = await response.Content.ReadAsByteArrayAsync();

	            return Encoding.Default.GetString(buffer);
	        }

	        return null;
	    }

        public string GetPairData(string pair)
	    {
			return GetPairDataAsync(pair).Result;
	    }
    }
}