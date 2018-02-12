
namespace Yobit.Api.Entities
{
	using System;
	using System.Collections.Generic;

	public class PairsInfo
	{
		public DateTimeOffset ServerTime { get; set; }
		public Dictionary<string, Pair> Pairs { get; set; }
    }
}