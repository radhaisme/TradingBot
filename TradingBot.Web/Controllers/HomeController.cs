using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Yobit.Api;

namespace TradingBot.Web.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetPairs()
		{
			return Content("");

			//var client = new YobitClient("https://yobit.net/api/3/", "https://yobit.net/tapi/", new YobitSettings
			//{
			//	Secret = "5ceeeb6072789d30e79a961335e63d50",
			//	ApiKey = "B03E731C650825B49CB2840E8449D98D",
			//	CreatedAt = new DateTime(2018, 1, 1)
			//});
			//string pairs = await client.GetPairsAsync();

			//return Content(pairs, "application/json");
		}

		[HttpGet]
		public async Task<IActionResult> GetPairData(string pair)
		{
			return Content("");
			//var client = new YobitClient("https://yobit.net/api/3/", "https://yobit.net/tapi/", new YobitSettings
			//{
			//	Secret = "5ceeeb6072789d30e79a961335e63d50",
			//	ApiKey = "B03E731C650825B49CB2840E8449D98D",
			//	CreatedAt = new DateTime(2018, 1, 1)
			//});
			//string data = await client.GetPairDataAsync(pair);

			//return Content(data, "application/json");
		}
	}
}