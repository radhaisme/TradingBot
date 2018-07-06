using Microsoft.AspNetCore.Mvc;

namespace TradingBot.Web.Controllers
{
	public class ArbitrageController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}