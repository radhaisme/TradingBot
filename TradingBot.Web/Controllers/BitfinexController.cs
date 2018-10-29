using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core.Proxies;
using TradingBot.Proxies;

namespace TradingBot.Web.Controllers
{
	public class BitfinexController : Controller
	{
		private readonly ILog _log;
		private readonly IProxy _proxy;

		public BitfinexController(ILog log)
		{
			_log = log;
			_proxy = new BitfinexProxy();
		}

		[HttpGet]
		public async Task<IActionResult> GetTradePairsAsync()
		{
			var response = await _proxy.GetTradePairsAsync();

			return Json(response);
		}
	}
}