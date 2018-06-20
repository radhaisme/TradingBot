
namespace Kucoin.Api
{
	public class KucoinClient
	{
		private readonly KucoinApi _api;
		private readonly IKucoinSettings _settings;

		public KucoinClient()
		{
			_settings = new KucoinSettings();
			_api = new KucoinApi(_settings.PublicUrl, _settings.PrivateUrl);
		}
	}
}