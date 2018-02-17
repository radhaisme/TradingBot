
namespace TradingBot.Core
{
	using Enums;
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Configuration;
    using System.Linq;
    using System.Reflection;

	public class ClientFactory : IClientFactory
	{
        private static string exchanges { get; set; }
        public static string GetExchanges
        {
            get
            {
                if (string.IsNullOrWhiteSpace(exchanges))
                {
                    exchanges = string.Join(", ", Clients.Select(m => string.Format("{0}-{1}", (int)m.Key, m.Key.ToString())));
                }
                return exchanges;
            }
        }

        public static Dictionary<AccountType, ExchangeInfo> Clients { get; private set; }

		static ClientFactory()
		{
			RegisterClientTypes();
		}

        public IExchangeClient Create(AccountType type, IApiSettings settings = null)
        {
            var client = Clients[type];
            return (IExchangeClient)Activator.CreateInstance(client.ExchangeApi, new object[] { client.PublicEndpoint, client.PrivateEndpoint, settings });
        }

		#region Private methods

		private static void RegisterClientTypes()
		{
            Clients = new Dictionary<AccountType, ExchangeInfo>();
            //NameValueCollection settings = ConfigurationManager.AppSettings;

            //foreach (string key in settings.Keys)
            //{
            //	var asm = Assembly.Load("Yobit.Api");
            //	var a = (AccountType)Enum.Parse(typeof(AccountType), key);
            //	Type clientType = asm.GetType(settings[key]);
            //	_clients.Add(a, clientType);
            //}

            //temporary:

            var asm = Assembly.Load("Yobit.Api");
            Type clientType = asm.GetType("Yobit.Api.YobitClient");
            Clients.Add(AccountType.Yobit, 
                new ExchangeInfo(AccountType.Yobit, "https://yobit.net/api/3/", "https://yobit.net/tapi", clientType));
        }

        #endregion
    }
}