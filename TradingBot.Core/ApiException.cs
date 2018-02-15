
namespace TradingBot.Core
{
	using System.Net.Http;

	public class ApiException : HttpRequestException
	{
		public ApiException(string message) : base(message)
		{ }
	}
}