using Bitmex.Api;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Web.Models;

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

			var client = new BitmexClient();
			var r = await client.GetTradePairsAsync();

			return Json(r);
		}
	}
}