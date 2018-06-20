
namespace Okex.Api
{
	public class OkexClient
	{
		private readonly OkexApi _api;
		private readonly IOkexSettings _settings;

		public OkexClient()
		{
			_settings = new OkexSettings();
			_api = new OkexApi(_settings.PublicUrl, _settings.PrivateUrl);
		}
	}
}