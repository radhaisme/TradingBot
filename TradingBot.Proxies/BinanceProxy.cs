using Binance.Api;
using System.Threading.Tasks;
using TradingBot.Core.Models;
using TradingBot.Core.Proxies;

namespace TradingBot.Proxies
{
	public sealed class BinanceProxy : IProxy
    {
		private readonly BinanceClient _client = new BinanceClient();

        public async Task<TradePairsResponse> GetTradePairsAsync()
        {
	        var response = await _client.GetTradePairsAsync();

            throw new System.NotImplementedException();
        }
    }
}