
namespace Cryptopia.Api
{
    public class CryptopiaClient
    {
	    private readonly CryptopiaApi _api;
	    private readonly ICryptopiaSettings _settings;

	    public CryptopiaClient()
	    {
			_settings = new CryptopiaSettings();
			_api = new CryptopiaApi(_settings.PublicUrl, _settings.PrivateUrl);
	    }
	}
}