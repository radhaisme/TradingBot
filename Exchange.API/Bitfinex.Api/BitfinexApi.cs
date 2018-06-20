
namespace Bitfinex.Api
{
	using System.Net.Http;
	using System.Threading.Tasks;
	using TradingBot.Common;
	using TradingBot.Core;

	public class BitfinexApi : ExchangeApi
    {
	    public BitfinexApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
	    { }

	    public async Task<string> GetCurrencies()
	    {
		    HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "/symbols");

		    if (response.IsSuccessStatusCode)
		    {
			    return await HttpHelper.AcquireStringAsync(response);
		    }

		    return null;
	    }

	    public async Task<string> GetPairsDetails()
	    {
		    HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "/symbols_details");

		    if (response.IsSuccessStatusCode)
		    {
			    return await HttpHelper.AcquireStringAsync(response);
		    }

		    return null;
		}

	    public async Task<string> GetPairDetail(string pair)
	    {
		    HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"/pubticker/{pair}");

		    if (response.IsSuccessStatusCode)
		    {
			    return await HttpHelper.AcquireStringAsync(response);
		    }

		    return null;
		}
    }
}