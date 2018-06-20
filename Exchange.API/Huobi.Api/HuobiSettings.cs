﻿using System;
using TradingBot.Core;

namespace Huobi.Api
{
	internal class HuobiSettings : ApiSettings, IApiSettings
	{
		public HuobiSettings() : base(typeof(IApiSettings))
		{ }

		public string PublicUrl { get; set; }
		public string PrivateUrl { get; set; }
		public string ApiKey { get; set; }
		public string Secret { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}