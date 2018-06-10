
namespace TradingBot.WPF.Models
{
	using System.Collections.Generic;

	public class StartViewModel
	{
		public StartViewModel()
		{
			Pairs = new List<string>();
		}

		public IEnumerable<string> Pairs { get; set; }
	}
}