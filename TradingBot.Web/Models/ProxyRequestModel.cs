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
        [Required]
        public string ModelType { get; set; }
        public string Body { get; set; }
	}
}