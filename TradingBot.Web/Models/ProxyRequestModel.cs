using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingBot.Web.Models
{
	public class ProxyRequestModel
    {
	    [Required]
	    public string ApiName { get; set; }
	    [Required]
	    public string Action { get; set; }
	    public IDictionary<string, string> Params { get; set; }
	}
}