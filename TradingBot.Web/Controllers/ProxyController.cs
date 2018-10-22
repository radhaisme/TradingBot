using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Web.Models;
using TradingBot.Proxies;

namespace TradingBot.Web.Controllers
{
	public class ProxyController : Controller
	{
		private readonly ILog _log;

		public ProxyController(ILog log)
		{
			_log = log;
		}

		[HttpPost]
		public async Task<IActionResult> Execute([FromBody]ProxyRequestModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var proxy = new BinanceProxy();
			var response = await proxy.GetTradePairsAsync();
			
			return Json(response);
		}
	}
}