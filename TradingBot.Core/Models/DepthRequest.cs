
namespace TradingBot.Core.Models
{
	public class DepthRequest
	{
		public string Pair { get; set; }
		public uint Limit { get; set; }
	}
}