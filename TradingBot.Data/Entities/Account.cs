
namespace TradingBot.Data.Entities
{
	using Enums;

	public class Account : Entity
	{
		public string Name { get; set; }

		public AccountType Type { get; set; }

		public string ApiKey { get; set; }

		public string Secret { get; set; }
	}
}