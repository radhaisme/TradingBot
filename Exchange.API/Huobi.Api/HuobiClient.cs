
namespace Huobi.Api
{
	public class HuobiClient
	{
		private readonly HuobiApi _api;
		private readonly IHuobiSettings _settings;

		public HuobiClient()
		{
			_settings = new HuobiSettings();
			_api = new HuobiApi(_settings.PublicUrl, _settings.PrivateUrl);
		}
	}
}