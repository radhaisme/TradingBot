
namespace TradingBot.Core
{
	using Enums;

	public class ClientAdapter : IClientAdapter
	{
		private IClientFactory _factory;
		private IExchangeClient _client;

		public ClientAdapter(IClientFactory factory)
		{
			_factory = factory;
		}

		public void SwitchAccount(AccountType type)
		{
			_client = _factory.Create(type, null);
		}
	}
}