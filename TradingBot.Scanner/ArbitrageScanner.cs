using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TradingBot.Core;
using TradingBot.Core.Entities;
using TradingBot.Core.Enums;

namespace TradingBot.Scanner
{
	public class ArbitrageScanner
	{
		private readonly List<IExchange> _exchanges = new List<IExchange>();
		private readonly List<ExchangePair> _exchangePairs = new List<ExchangePair>();
		private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

		public ArbitrageScanner(IExchangeFactory factory)
		{
			CreateExchanges(factory);
			GenerateExchangePairs();
		}

		public async void Start()
		{
			var output = new ConcurrentBag<ArbitrageInfo>();
			var tasks = new List<Task>();

			foreach (ExchangePair pair in _exchangePairs)
			{
				var task = Task.Run(() => ProcessItem(pair, output), _tokenSource.Token);
				tasks.Add(task);
			}

			await Task.WhenAll(tasks);
		}

		private async Task ProcessItem(ExchangePair exchangePair, IProducerConsumerCollection<ArbitrageInfo> output)
		{
			foreach (Pair pair in exchangePair.Pairs)
			{
				(decimal buy, decimal sell) first = await exchangePair.First.GetBookOrderPriceAsync(pair);
				(decimal buy, decimal sell) second = await exchangePair.Second.GetBookOrderPriceAsync(pair);

				if (first.buy == 0 && first.sell == 0 || second.buy == 0 && second.sell == 0)
				{
					continue;
				}

				//decimal first = await exchangePair.First.GetPriceAsync(pair);
				//decimal second = await exchangePair.Second.GetPriceAsync(pair);

				var model = new ArbitrageInfo();
				model.Symbol = pair.Label;
				//model.BuyPrice = first < second ? first : second;
				//model.SellPrice = first > second ? first : second;

				model.BuyPrice = first.sell > second.sell ? second.sell : first.sell;
				model.SellPrice = first.buy > second.buy ? first.buy : second.buy;

				if (model.BuyPrice < model.SellPrice)
				{
					model.Route = $"{exchangePair.Second.Type}->{exchangePair.First.Type}";
					model.Rate = GetPercent(model.SellPrice, model.BuyPrice);
				}
				else
				{
					model.Route = $"{exchangePair.First.Type}->{exchangePair.Second.Type}";
					model.Rate = GetPercent(model.BuyPrice, model.SellPrice);
				}

				output.TryAdd(model);
				Thread.Sleep(1000);
			}
		}

		private float GetPercent(decimal a, decimal b)
		{
			return (float)((a - b) / b * 100);
		}

		public void Stop()
		{
			_tokenSource.Cancel();
		}

		#region Private methods

		private void CreateExchanges(IExchangeFactory factory)
		{
			_exchanges.Add(factory.Create(ExchangeType.Binance));
			_exchanges.Add(factory.Create(ExchangeType.Huobi));
			_exchanges.Add(factory.Create(ExchangeType.Yobit));
			_exchanges.Add(factory.Create(ExchangeType.Bitfinex));
			_exchanges.Add(factory.Create(ExchangeType.Cryptopia));
			_exchanges.Add(factory.Create(ExchangeType.Kucoin));
			_exchanges.Add(factory.Create(ExchangeType.Exmo));
			_exchanges.Add(factory.Create(ExchangeType.Okex));
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
}