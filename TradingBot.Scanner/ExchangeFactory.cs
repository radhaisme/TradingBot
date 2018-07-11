using Binance.Api;
using Bitfinex.Api;
using Cryptopia.Api;
using Exmo.Api;
using Huobi.Api;
using Kucoin.Api;
using Okex.Api;
using System.Collections.Generic;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;
using Yobit.Api;

namespace TradingBot.Scanner
{
	public class ExchangeFactory : IExchangeFactory
	{
		private readonly Dictionary<ExchangeType, IExchange> _exchanges = new Dictionary<ExchangeType, IExchange>();
		private readonly IReadOnlyDictionary<string, IList<Currency>> _currencies;

		public ExchangeFactory(ICurrencyProvider provider)
		{
			_currencies = provider.GetCurrenciesAsync().Result;
			//TODO: Add logic for finding registered exchanges
		}

		public IExchange Create(ExchangeType type)
		{
			if (_exchanges.ContainsKey(type))
			{
				return _exchanges[type];
			}

			switch (type)
			{
				case ExchangeType.Binance:
					{
						IExchange ex = new Exchange(_currencies, new BinanceClient());
						ex.Type = type;
						ex.Initialize();
						_exchanges.Add(type, ex);

						return ex;
					}
				case ExchangeType.Huobi:
					{
						IExchange ex = new Exchange(_currencies, new HuobiClient());
						ex.Type = type;
						ex.Initialize();
						_exchanges.Add(type, ex);

						return ex;
					}
				case ExchangeType.Yobit:
					{
						IExchange ex = new Exchange(_currencies, new YobitClient());
						ex.Type = type;
						ex.Initialize();
						_exchanges.Add(type, ex);

						return ex;
					}
				case ExchangeType.Bitfinex:
					{
						IExchange ex = new Exchange(_currencies, new BitfinexClient());
						ex.Type = type;
						ex.Initialize();
						_exchanges.Add(type, ex);

						return ex;
					}
				case ExchangeType.Kucoin:
					{
						IExchange ex = new Exchange(_currencies, new KucoinClient());
						ex.Type = type;
						ex.Initialize();
						_exchanges.Add(type, ex);

						return ex;
					}
				case ExchangeType.Okex:
					{
						IExchange ex = new Exchange(_currencies, new OkexClient());
						ex.Type = type;
						ex.Initialize();
						_exchanges.Add(type, ex);

						return ex;
					}
				case ExchangeType.Cryptopia:
					{
						IExchange ex = new Exchange(_currencies, new CryptopiaClient());
						ex.Type = type;
						ex.Initialize();
						_exchanges.Add(type, ex);

						return ex;
					}
				case ExchangeType.Exmo:
					{
						IExchange ex = new Exchange(_currencies, new ExmoClient());
						ex.Type = type;
						ex.Initialize();
						_exchanges.Add(type, ex);

						return ex;
					}
				default:
					{
						return null;
					}
			}
		}
	}
}