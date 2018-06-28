using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core.Entities;

namespace Huobi.Api
{
	public class HuobiClient
	{
		private readonly HuobiApi _api;
		private readonly IHuobiSettings _settings;

		public HuobiClient()
		{
			_settings = new HuobiSettings();
			_api = new HuobiApi(_settings.PublicUrl, _settings.PrivateUrl);
		}

		public async Task<IEnumerable<Pair>> GetPairs()
		{
			HttpResponseMessage response = await _api.GetPairs();
			var content = await HttpHelper.AcquireContentAsync<dynamic>(response);
			var pairs = new List<Pair>();



			return pairs;
		}
	}
}