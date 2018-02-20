
namespace Yobit.Api
{
	using System;

	public sealed class YobitSettings : IYobitSettings
	{
		public string ApiKey { get; set; }
		public string Secret { get; set; }
		public DateTimeOffset CreatedAt { get; set; }

	}
}