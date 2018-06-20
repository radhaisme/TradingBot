
namespace Bitfinex.Api
{
	public class BitfinexClient
	{
		private readonly BitfinexApi _api;
		private readonly IBitfinexSettings _settings;

		public BitfinexClient()
		{
			_settings = new BitfinexSettings();
			_api = new BitfinexApi(_settings.PublicUrl, _settings.PrivateUrl);
		}
	}
}