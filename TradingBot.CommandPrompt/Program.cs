using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Binance.Api;
using Huobi.Api;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;
using TradingBot.CurrencyProvider;
using Pair = TradingBot.Data.Entities.Pair;
using SymbolFormatter = Huobi.Api.SymbolFormatter;

namespace TradingBot.CommandPrompt
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			var provider = new CoinMarketCapClient();
			var factory = new ExchangeFactory(provider);
			var scanner = new ArbitrageScanner(factory);
			scanner.Start();

			Console.ReadLine();
		}
	}

	public class ExchangePair
	{
		public ExchangePair(IExchange first, IExchange second)
		{
			First = first;
			Second = second;
			Pairs = new ReadOnlyCollection<Pair>(first.Pairs.Intersect(second.Pairs).ToList());
		}

		public IReadOnlyCollection<Pair> Pairs { get; }
		public IExchange First { get; }
		public IExchange Second { get; }

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

	public class ArbitrageInfo
	{
		public string Symbol { get; set; }
		public string Route { get; set; }
		public decimal Ask { get; set; }
		public decimal Bid { get; set; }
		public decimal Percent { get; set; }

		public override string ToString()
		{
			return $"{Symbol};{Route};{Ask};{Bid};{Percent:0.##}";
		}
	}

	public class ArbitrageScanner
	{
		private readonly List<IExchange> _exchanges = new List<IExchange>();
		private readonly List<ExchangePair> _exchangePairs = new List<ExchangePair>();

		public ArbitrageScanner(IExchangeFactory factory)
		{
			CreateExchanges(factory);
			GenerateExchangePairs();
		}

		public void Start()
		{
			var exchangePair = _exchangePairs.First();
			var prices = new Dictionary<Pair, ArbitrageInfo>();

			foreach (Pair pair in exchangePair.Pairs)
			{
				decimal first = exchangePair.First.GetPrice(pair);
				decimal second = exchangePair.Second.GetPrice(pair);
				var info = new ArbitrageInfo();
				info.Symbol = pair.Label;

				if (first > second)
				{
					info.Route = $"Binance->Huobi";
					info.Bid = first;
					info.Ask = second;
					info.Percent = (first - second) / second * 100;
				}
				else
				{
					info.Route = $"Huobi->Binance";
					info.Bid = second;
					info.Ask = first;
					info.Percent = (second - first) / first * 100;
				}

				prices.Add(pair, info);
				Thread.Sleep(500);
			}

			using (StreamWriter sw = File.CreateText("Arbitrage.log"))
			{
				foreach (ArbitrageInfo info in prices.Values)
				{
					sw.WriteLine(info);
				}
			}
		}

		public void Stop()
		{

		}

		#region Private methods

		private void CreateExchanges(IExchangeFactory factory)
		{
			_exchanges.Add(factory.Create(ExchangeType.Binance));
			_exchanges.Add(factory.Create(ExchangeType.Huobi));
		}

		private void GenerateExchangePairs()
		{
			for (var i = 0; i < _exchanges.Count; i++)
			{
				for (var j = i + 1; j < _exchanges.Count; j++)
				{
					var exchangePair = new ExchangePair(_exchanges[i], _exchanges[j]);
					_exchangePairs.Add(exchangePair);
				}
			}
		}

		#endregion
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

		public ExchangeType Type { get; set; }

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

		public decimal GetPrice(Pair pair)
		{
			ISymbolFormatter formatter;

			if (Type == ExchangeType.Binance)
			{
				formatter = new Binance.Api.SymbolFormatter();
			}
			else
			{
				formatter = new Huobi.Api.SymbolFormatter();
			}

			PairDetail detail = _client.GetPairDetailAsync(pair.GetSymbol(formatter)).Result;

			return detail.LastPrice;
		}
	}

	public interface IExchange
	{
		ExchangeType Type { get; set; }
		IReadOnlyCollection<Pair> Pairs { get; }
		void Initialize();
		decimal GetPrice(Pair pair);
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