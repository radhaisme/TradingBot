using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Proxies;
using TradingBot.Web.Models;

namespace TradingBot.Web.Controllers
{
	[Route("api/proxy")]
    public class ProxyController : Controller
    {
	    //private readonly ILog _log;

	    //public ProxyController(ILog log)
	    //{
		   // _log = log;
	    //}

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