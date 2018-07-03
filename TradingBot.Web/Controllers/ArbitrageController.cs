using System.Collections;
using Binance.Api;
using Huobi.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;
using Pair = TradingBot.Data.Entities.Pair;

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