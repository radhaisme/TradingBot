using Newtonsoft.Json;

namespace Huobi.Api
{
	internal class ErrorModel
	{
		public string Status { get; set; }
		[JsonProperty("err-code")]
		public string ErrCode { get; set; }
		[JsonProperty("err-msg")]
		public string ErrMsg { get; set; }
	}
}