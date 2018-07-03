using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TradingBot.Core;
using TradingBot.Core.Entities;

namespace TradingBot.Web.Controllers
{
	public class ArbitrageController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}

	public class ExchangePair
	{
		private readonly IExchangeClient _first;
		private readonly IExchangeClient _second;
		private readonly IEnumerable<Pair> _pairs;

		public ExchangePair(IExchangeClient first, IExchangeClient second)
		{
			_first = first;
			_second = second;
			_pairs = new List<Pair>();
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
		private List<ExchangePair> _pairs = new List<ExchangePair>();

		public ArbitrageScanner()
		{

		}
	}

	public class Exchange : IExchange
	{
		private readonly IExchangeClient _client;
		private static IReadOnlyDictionary<string, IList<Currency>> _currencies;

		public Exchange(IReadOnlyDictionary<string, IList<Currency>> currencies, IExchangeClient client)
		{
			_currencies = currencies;
			_client = client;
		}

		public async void Initialize()
		{
			var pairs = await _client.GetPairsAsync();


		}
	}

	public interface IExchange
	{
		void Initialize();
	}

	public class ExchangeFactory : IExchangeFactory
	{
		public ExchangeFactory()
		{

		}

		public IExchange Create()
		{
			return null;
		}
	}

	public interface IExchangeFactory
	{
		IExchange Create();
	}
}