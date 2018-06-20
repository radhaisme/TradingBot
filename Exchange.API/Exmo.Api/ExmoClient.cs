
namespace Exmo.Api
{
	public class ExmoClient
	{
		private readonly ExmoApi _api;
		private readonly IExmoSettings _settings;

		public ExmoClient()
		{
			_settings = new ExmoSettings();
			_api = new ExmoApi(_settings.PublicUrl, _settings.PrivateUrl);
		}
	}
}