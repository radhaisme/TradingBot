﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Huobi.Api.Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TradeType : byte
	{
		Buy,
		Sell
	}
}