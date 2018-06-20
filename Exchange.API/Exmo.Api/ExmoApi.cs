using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;

namespace Exmo.Api
{
	public class ExmoApi : ExchangeApi
    {
	    public ExmoApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
	    { }

	    public async Task<string> GetPairsDetails()
	    {
		    HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "ticker");

		    if (response.IsSuccessStatusCode)
		    {
			    return await HttpHelper.AcquireStringAsync(response);
		    }

		    return null;
	    }

	    public async Task<string> GetPairSettings()
	    {
		    HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "pair_settings");

		    if (response.IsSuccessStatusCode)
		    {
			    return await HttpHelper.AcquireStringAsync(response);
		    }

		    return null;
		}

	    public async Task<string> GetCurrencies()
	    {
		    HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "currency");

		    if (response.IsSuccessStatusCode)
		    {
			    return await HttpHelper.AcquireStringAsync(response);
		    }

		    return null;
		}
    }
}