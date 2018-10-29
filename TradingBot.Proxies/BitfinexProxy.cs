using AutoMapper;
using Bitfinex.Api;
using System.Threading.Tasks;
using TradingBot.Core.Models;
using TradingBot.Core.Proxies;

namespace TradingBot.Proxies
{
	public class BitfinexProxy : IProxy
	{
		private readonly BitfinexClient _client;
		private readonly IMapper _mapper;

		public BitfinexProxy()
		{
			_client = new BitfinexClient();
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<BitfinexProfile>();
			});
			_mapper = config.CreateMapper();
		}

		public async Task<TradePairsResponse> GetTradePairsAsync()
		{
			var response = await _client.GetTradePairsAsync();
			
			return _mapper.Map<TradePairsResponse>(response);
		}
	}

	public class BitfinexProfile : Profile
	{
		public BitfinexProfile()
		{
			CreateMap<Bitfinex.Api.Models.TradePairsResponse, TradePairsResponse>();
			CreateMap<Bitfinex.Api.Models.TradePairResult, TradePair>().ConstructUsing(x => new TradePair(new Currency(x.BaseAsset), new Currency(x.QuoteAsset)));
		}
	}
}