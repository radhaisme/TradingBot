
using System;

namespace Yobit.Api
{
	public sealed class YobitSettings : IYobitSettings
	{
		public string ApiKey { get; set; }
		public string Secret { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}