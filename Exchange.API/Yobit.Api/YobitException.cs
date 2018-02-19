
namespace Yobit.Api
{
	using TradingBot.Core;

	public class YobitException : ApiException
	{
		public bool IsPrivate { get; private set; }

		public YobitException(string message) : base(message)
		{ }
	}
}