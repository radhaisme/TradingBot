
namespace Kucoin.Api
{
	internal class ResponseModel
	{
		public string Code { get; set; }
		public string Msg { get; set; }
		public bool Success { get; set; }
		public dynamic Data { get; set; }
	}
}