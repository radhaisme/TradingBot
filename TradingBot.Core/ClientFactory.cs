
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
                    exchanges = string.Join(", ", _clients.Select(m => string.Format("{0}-{1}", (int)m.Key, m.Key.ToString())));
                }
                return exchanges;
            }
        }

        private static Dictionary<Exchange, ExchangeInfo> _clients { get; set; }

        static ClientFactory()
		{
			RegisterClientTypes();
		}

        public static bool IsRegistered(Exchange exchange)
        {
            return _clients.ContainsKey(exchange);
        }

        public IExchangeClient Create(Exchange type, IApiSettings settings = null)
        {
            if (!IsRegistered(type))
                throw new ArgumentOutOfRangeException("Such Exchange is not registered.");
            var client = _clients[type];
            return (IExchangeClient)Activator.CreateInstance(client.ExchangeApi, new object[] { client.PublicEndpoint, client.PrivateEndpoint, settings });
        }

		#region Private methods

		private static void RegisterClientTypes()
		{
            _clients = new Dictionary<Exchange, ExchangeInfo>();
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
            _clients.Add(Exchange.Yobit, 
                new ExchangeInfo(Exchange.Yobit, "https://yobit.net/api/3/", "https://yobit.net/tapi", clientType));
        }

        #endregion
    }
}