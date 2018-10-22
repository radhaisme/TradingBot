using System.Collections.Generic;
using Bitmex.Api;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Binance.Api;
using TradingBot.Common;
using TradingBot.Core.Enums;
using TradingBot.Web.Models;
using System;

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

			var client = new BinanceClient();
			var r = await client.GetTradePairsAsync();

			return Json(r);
		}
	}

    public static class ProxyFactory
    {
        private static readonly IDictionary<ExchangeType, Type> _connectors = new Dictionary<ExchangeType, Type>();

        static ProxyFactory()
        {
            _connectors.Add(ExchangeType.Binance, typeof(BinanceClient));
        }

        public static object GetConnector(ExchangeType type)
        {
            return null;
        }
    }
}