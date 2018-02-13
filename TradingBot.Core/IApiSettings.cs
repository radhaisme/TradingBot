
namespace TradingBot.Core
{
	public interface IApiSettings
    {
		string PublicKey { get; set; }
	    string Secret { get; set; }
	    string BaseAddress { get; set; }
	    string ApiPrefix { get; set; }
	}
}