namespace TradingBot.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Core;
	using Core.Enums;

	public class ExchangeInfo
    {
        public Exchange Type { get; set; }
        
        public string PublicEndpoint { get; set; }

        public string PrivateEndpoint { get; set; }

        public Type ExchangeApi { get; set; } 

        public ExchangeInfo(Exchange type, string publicEndpoint, string privateEndpoint, Type excangeApi)
        {
            Type = type;
            PrivateEndpoint = privateEndpoint;
            PublicEndpoint = publicEndpoint;
            ExchangeApi = excangeApi;
        }
    }

}