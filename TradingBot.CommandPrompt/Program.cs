using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Binance.Api;
using Huobi.Api;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;
using TradingBot.CurrencyProvider;
using Pair = TradingBot.Data.Entities.Pair;

namespace TradingBot.CommandPrompt
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			var provider = new CoinMarketCapClient();
			var factory = new ExchangeFactory(provider);
			var scanner = new ArbitrageScanner(factory);
			
			Console.ReadLine();
		}
	}

	public class ExchangePair
	{
		private readonly IExchange _first;
		private readonly IExchange _second;
		private readonly IEnumerable<Pair> _pairs;

		public ExchangePair(IExchange first, IExchange second)
		{
			_first = first;
			_second = second;
			_pairs = first.Pairs.Intersect(second.Pairs).ToList();
		}

		public void Transfer(string symbol, bool swap)
		{
			if (swap)
			{
				//TODO: Swap exchanges

				return;
			}

			//TODO: Transfer coins from the firs to the second exchange
		}
	}

	public class ArbitrageScanner
	{
		private readonly List<IExchange> _exchanges = new List<IExchange>();
		private readonly List<ExchangePair> _exchangePairs = new List<ExchangePair>();

		public ArbitrageScanner(IExchangeFactory factory)
		{
			_exchanges.Add(factory.Create(ExchangeType.Binance));
			_exchanges.Add(factory.Create(ExchangeType.Huobi));

			for (var i = 0; i < _exchanges.Count; i++)
			{
				for (var j = i + 1; j < _exchanges.Count; j++)
				{
					var exchangePair = new ExchangePair(_exchanges[i], _exchanges[j]);
					_exchangePairs.Add(exchangePair);
				}
			}
		}
	}

	public class Exchange : IExchange
	{
		private readonly IExchangeClient _client;
		private readonly IReadOnlyDictionary<string, IList<Currency>> _currencies;

		public Exchange(IReadOnlyDictionary<string, IList<Currency>> currencies, IExchangeClient client)
		{
			_currencies = currencies;
			_client = client;
		}

		public IReadOnlyCollection<Pair> Pairs { get; private set; }

		void IExchange.Initialize()
		{
			var pairs = new List<Pair>();

			foreach (Core.Entities.Pair item in _client.GetPairsAsync().Result)
			{
				if (!_currencies.ContainsKey(item.BaseAsset) || !_currencies.ContainsKey(item.QuoteAsset))
				{
					continue;
				}

				IList<Currency> bases = _currencies[item.BaseAsset];
				IList<Currency> quotes = _currencies[item.QuoteAsset];

				if (bases.Count > 1 || quotes.Count > 1)
				{
					continue;
				}

				var pair = new Pair(bases[0], quotes[0]);
				pairs.Add(pair);
			}

			Pairs = new ReadOnlyCollection<Pair>(pairs);
		}
	}

	public interface IExchange
	{
		IReadOnlyCollection<Pair> Pairs { get; }
		void Initialize();
	}

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
						ex.Initialize();
						_exchanges.Add(type, ex);

						return ex;
					}
				case ExchangeType.Huobi:
					{
						IExchange ex = new Exchange(_currencies, new HuobiClient());
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

	public interface IExchangeFactory
	{
		IExchange Create(ExchangeType type);
	}
}