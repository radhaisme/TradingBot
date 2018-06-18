
namespace Huobi.Api
{
	using System.Net.Http;
	using System.Threading.Tasks;
	using TradingBot.Common;
	using TradingBot.Core;

	public sealed class HuobiApi : ExchangeApi
    {
	    public HuobiApi(string publicEndpoint, string privateEndpoint) : base(publicEndpoint, privateEndpoint)
	    { }

	    public async Task<string> GetCurrencies()
	    {
		    HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + "v1/common/symbols");

		    if (response.IsSuccessStatusCode)
		    {
			    return await HttpHelper.AcquireStringAsync(response);
		    }

		    return null;
	    }

	    public async Task<string> GetPairDetail(string pair)
	    {
		    HttpResponseMessage response = await HttpClient.GetAsync(PublicUrl + $"market/detail/merged?symbol={pair}");

		    if (response.IsSuccessStatusCode)
		    {
			    return await HttpHelper.AcquireStringAsync(response);
		    }

		    return null;
	    }
    }
}