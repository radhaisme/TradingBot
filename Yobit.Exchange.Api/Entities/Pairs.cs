
namespace Yobit.Exchange.Api.Entities
{
	using System;
	using System.Collections.Generic;

	public class PairsInfo
	{
		public DateTimeOffset ServerTime { get; set; }
		public List<Pair> Pairs { get; set; }
    }
}