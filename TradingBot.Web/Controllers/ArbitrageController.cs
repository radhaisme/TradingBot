using log4net;
using Microsoft.AspNetCore.Mvc;

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
			return View();
		}
	}
}