
namespace Yobit.Api
{
	using Newtonsoft.Json;

	public class YobitResponse<TModel>
    {
		public bool Success { get; set; }
	    [JsonProperty(PropertyName = "@return")]
		public TModel Content { get; set; }
		public string Error { get; set; }
    }
}