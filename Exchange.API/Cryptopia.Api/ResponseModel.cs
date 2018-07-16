
namespace Cryptopia.Api
{
    internal class ResponseModel
    {
		public bool Success { get; set; }
		public string Message { get; set; }
		public dynamic Data { get; set; }
		public string Error { get; set; }
	}
}