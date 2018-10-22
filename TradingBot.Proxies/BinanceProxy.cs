using AutoMapper;
using Binance.Api;
using System.Threading.Tasks;
using TradingBot.Core.Models;
using TradingBot.Core.Proxies;

namespace TradingBot.Proxies
{
	public sealed class BinanceProxy : IProxy
	{
		private readonly BinanceClient _client;
		private readonly IMapper _mapper;

		public BinanceProxy()
		{
			_client = new BinanceClient();
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<BinanceProfile>();
			});
			_mapper = config.CreateMapper();
		}
		
		public async Task<TradePairsResponse> GetTradePairsAsync()
		{
			var response = await _client.GetTradePairsAsync();

			return _mapper.Map<TradePairsResponse>(response);
		}
	}

	public class BinanceProfile : Profile
	{
		public BinanceProfile()
		{
			CreateMap<Binance.Api.Models.TradePairsResponse, TradePairsResponse>();
			CreateMap<Binance.Api.Models.TradePairResult, TradePair>().ConstructUsing(x => new TradePair(new Currency(x.BaseAsset), new Currency(x.QuoteAsset)));
		}
	}
}