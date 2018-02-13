
namespace TradingBot.Common
{
	using Newtonsoft.Json;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net.Http;

    public static class HttpHelper
	{
        public static async Task<TModel> AcquireContentAsync<TModel>(HttpResponseMessage message)
        {
            if (message.IsSuccessStatusCode)
            {
                byte[] buffer = await message.Content.ReadAsByteArrayAsync();
                string json = Encoding.Default.GetString(buffer);
                var result = JsonHelper.FromJson<TModel>(json);

                return result;
            }

            return default;
        }
    }
}