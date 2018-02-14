
namespace TradingBot.Common
{
	using System.Net.Http;
	using System.Text;
	using System.Threading.Tasks;

	public static class HttpHelper
	{
		public static async Task<TModel> AcquireContentAsync<TModel>(HttpResponseMessage message)
		{
			//if (!message.IsSuccessStatusCode)
			//{
			//	throw new HttpRequestException(await AcquireStringAsync(message));
			//}

			string json = await AcquireStringAsync(message);
			var result = JsonHelper.FromJson<TModel>(json);

			return result;
		}

		public static async Task<string> AcquireStringAsync(HttpResponseMessage message)
		{
			byte[] buffer = await message.Content.ReadAsByteArrayAsync();
			string json = Encoding.Default.GetString(buffer);

			return json;
		}
	}
}