using Microsoft.AspNetCore.Mvc;
using TradingBot.Common;

namespace TradingBot.Web.Controllers
{
	public class ArbitrageController : Controller
	{
		private readonly ILog _log;

		public ArbitrageController(ILog log)
		{
			_log = log;
		}

		public IActionResult Index()
		{
			_log.Info("dasda");

			return View();
		}
	}
}