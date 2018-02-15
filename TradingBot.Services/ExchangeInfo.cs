namespace TradingBot.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Core;
	using Core.Enums;
	using Yobit.Api;

	public class ExchangeInfo
    {
        private static string accountTypes { get; set; }
        public static string GetAccountTypes
        {
            get
            {
                if (string.IsNullOrWhiteSpace(accountTypes))
                {
                    accountTypes = string.Join(", ", Exchanges.Select(m => string.Format("{0}-{1}", (int)m.Key, m.Key.ToString())));
                }
                return accountTypes;
            }
        }

        public static Dictionary<AccountType, ExchangeInfo> Exchanges { get; set; }

        static ExchangeInfo()
        {
            Exchanges = new Dictionary<AccountType, ExchangeInfo>();
            Exchanges.Add(AccountType.Yobit, new ExchangeInfo(AccountType.Yobit, "https://yobit.net/api/3/", "https://yobit.net/tapi", typeof(Yobit.Api.YobitApi)));
            //Exchanges.Add(AccountType.Bitfinex, new ExchangeInfo(AccountType.Bitfinex, "https://api.bitfinex.com/v2/", "https://api.bitfinex.com/v2/", typeof(Yobit.Api.YobitApi)));
        }

        public AccountType Type { get; set; }
        
        public string PublicEndpoint { get; set; }

        public string PrivateEndpoint { get; set; }

        public Type ExcangeApi { get; set; } 

        public ExchangeApi Api
        {
            get { return (ExchangeApi)Activator.CreateInstance(ExcangeApi, new[] { PublicEndpoint, PrivateEndpoint }); }
        }

	    public static YobitClient Client //Пока шо так, до тех пор пока нет точного набора методов с бирж, их описания и сущностей с абстракциями
	    {
		    get
		    {
			    var settings = new YobitSettings();
			    settings.BaseAddress = "https://yobit.net";
			    
				return new YobitClient(settings);
		    }
	    }

        public ExchangeInfo(AccountType type, string publicEndpoint, string privateEndpoint, Type excangeApi)
        {
            Type = type;
            PrivateEndpoint = privateEndpoint;
            PublicEndpoint = publicEndpoint;
            ExcangeApi = excangeApi;
        }
    }

}