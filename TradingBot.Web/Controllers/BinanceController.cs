using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core.Proxies;
using TradingBot.Proxies;

namespace TradingBot.Web.Controllers
{
	public class BinanceController : Controller
	{
		private readonly ILog _log;
		private readonly IProxy _proxy;

		public BinanceController(ILog log)
		{
			_log = log;
			_proxy = new BinanceProxy();
		}

		[HttpGet]
		public async Task<IActionResult> GetTradePairsAsync()
		{
			var response = await _proxy.GetTradePairsAsync();

			return Json(response);
		}
	}
}