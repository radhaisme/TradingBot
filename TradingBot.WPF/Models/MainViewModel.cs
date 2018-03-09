
namespace TradingBot.WPF.Models
{
	using Core.Entities;
	using System.Collections.Generic;

	public class MainViewModel
	{
		public MainViewModel()
		{
			Pairs = new Dictionary<string, List<Pair>>();
		}

		public IDictionary<string, List<Pair>> Pairs { get; set; }
	}
}