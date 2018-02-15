
namespace TradingBot.Core
{
	using Enums;
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Configuration;
	using System.Reflection;

	public class ClientFactory : IClientFactory
	{
		private static Dictionary<AccountType, Type> _clients = new Dictionary<AccountType, Type>();

		public ClientFactory()
		{
			RegisterClientTypes();
		}

		public IExchangeClient Create(AccountType type)
		{
			return (IExchangeClient)Activator.CreateInstance(_clients[type]);
		}

		#region Private methods

		private void RegisterClientTypes()
		{
			NameValueCollection settings = ConfigurationManager.AppSettings;

			foreach (string key in settings.Keys)
			{
				var asm = Assembly.Load("Yobit.Api");
				var a = (AccountType)Enum.Parse(typeof(AccountType), key);
				Type clientType = asm.GetType(settings[key]);
				_clients.Add(a, clientType);
			}
		}

		#endregion
	}
}