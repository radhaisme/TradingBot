using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradingBot.Web.Models;
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
		public IActionResult GetPairs()
		{
			var _client = new YobitClient("https://yobit.net/api/3/", "https://yobit.net/tapi/", new YobitSettings
			{
				Secret = "5ceeeb6072789d30e79a961335e63d50",
				ApiKey = "B03E731C650825B49CB2840E8449D98D",
				CreatedAt = new DateTime(2018, 1, 1)
			});

			var pairs = _client.GetPairsAsync().Result;

			return Json(pairs.Pairs);
		}
    }
}
