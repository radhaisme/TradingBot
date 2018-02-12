using System.Collections.Generic;
using System.Linq;

namespace TradingBot.Core
{
	using Data.Enums;

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
            Exchanges.Add(AccountType.Yobit, new ExchangeInfo(AccountType.Yobit, "https://yobit.net/api/3/"));
            Exchanges.Add(AccountType.Bitfinex, new ExchangeInfo(AccountType.Bitfinex, "https://api.bitfinex.com/v2/"));
        }

        public AccountType Type { get; set; }
        
        public string BasicUrl { get; set; }

        public ExchangeInfo(AccountType type, string basicUrl)
        {
            Type = type;
            BasicUrl = basicUrl;
        }
    }

}