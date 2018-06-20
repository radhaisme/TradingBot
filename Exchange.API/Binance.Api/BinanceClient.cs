
namespace Binance.Api
{
    public sealed class BinanceClient
    {
	    private readonly BinanceApi _api;
	    private readonly IBinanceSettings _settings;

	    public BinanceClient()
	    {
			_settings = new BinanceSettings();
			_api = new BinanceApi(_settings.PublicUrl, _settings.PrivateUrl);
	    }
    }
}