
namespace Yobit.Api
{
	using Newtonsoft.Json;

	[JsonConverter(typeof(YobitResponseConverter))]
	public class YobitResponse
    {
	    public bool Success { get; set; }
	    [JsonProperty(PropertyName = "@return")]
		public string Content { get; set; }
		public string Error { get; set; }
    }
}