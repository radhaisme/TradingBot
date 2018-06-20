﻿using System;
using TradingBot.Core;

namespace Yobit.Api
{
	internal sealed class YobitSettings : ApiSettings, IYobitSettings
	{
		public YobitSettings() : base(typeof(IYobitSettings))
		{ }

		public string PublicUrl { get; set; }
		public string PrivateUrl { get; set; }
		public string ApiKey { get; set; }
		public string Secret { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}