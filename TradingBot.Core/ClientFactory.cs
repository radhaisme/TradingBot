
namespace TradingBot.Core
{
	using Enums;
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public class ClientFactory : IClientFactory
	{
		private static Dictionary<ExchangeType, ExchangeInfo> Clients { get; set; }
		
		static ClientFactory()
		{
			Clients = new Dictionary<ExchangeType, ExchangeInfo>();
			RegisterClientTypes();
		}

		public static IEnumerable<string> GetExchanges()
		{
			foreach (KeyValuePair<ExchangeType, ExchangeInfo> client in Clients)
			{
				yield return String.Format("{0}-{1}", (int)client.Key, client.Key.ToString());
			}
		}

		public static bool IsExists(ExchangeType exchangeType)
		{
			return Clients.ContainsKey(exchangeType);
		}

		//public IApiClient Create(ExchangeType type, IApiSettings settings = null)
		//{
		//	if (!IsExists(type))
		//	{
		//		throw new ArgumentOutOfRangeException("Such Exchange is not registered.");
		//	}

		//	ExchangeInfo client = Clients[type];

		//	return (IApiClient)Activator.CreateInstance(client.ExchangeApi, client.PublicEndpoint, client.PrivateEndpoint, settings);
		//}

		#region Private methods

		private static void RegisterClientTypes()
		{
			//NameValueCollection settings = ConfigurationManager.AppSettings;

			//foreach (string key in settings.Keys)
			//{
			//	var asm = Assembly.Load("Yobit.Api");
			//	var a = (AccountType)Enum.Parse(typeof(AccountType), key);
			//	Type clientType = asm.GetType(settings[key]);
			//	_clients.Add(a, clientType);
			//}

			//temporary:

			//var assembly = Assembly.Load("Yobit.Api");
			//Type clientType = assembly.GetType("Yobit.Api.YobitClient");
			//Clients.Add(ExchangeType.Yobit, new ExchangeInfo("https://yobit.net/api/3/", "https://yobit.net/tapi", clientType));
		}

		#endregion
	}
}