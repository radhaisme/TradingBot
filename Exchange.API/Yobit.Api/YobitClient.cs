
namespace Yobit.Api
{
	using TradingBot.Core;

	public sealed class YobitClient : IExchangeClient
	{
		private YobitApi _api;
		private IYobitSettings _settings;

		public YobitClient(IYobitSettings settings)
		{
			_settings = settings;
			_api = new YobitApi(settings);
		}
	}
}