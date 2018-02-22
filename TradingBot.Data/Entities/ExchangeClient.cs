
namespace TradingBot.Data.Entities
{
	using Core.Enums;

	public class ExchangeClient : Entity
	{
		public string FullName { get; set; }
		public string Path { get; set; }
		public ExchangeType ExchangeType { get; set; }
	}
}