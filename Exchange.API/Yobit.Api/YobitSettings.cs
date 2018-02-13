
namespace Yobit.Api
{
	public sealed class YobitSettings : IYobitSettings
	{
		public string PublicKey { get; set; }
		public string Secret { get; set; }
		public string BaseAddress { get; set; }
		public string ApiPrefix { get; set; }
		public int Counter { get; set; }
	}
}