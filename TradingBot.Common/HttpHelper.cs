using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TradingBot.Common
{
	public static class HttpHelper
	{
		public static async Task<TModel> AcquireContentAsync<TModel>(HttpResponseMessage message)
		{
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

		public static string QueryString(IDictionary<string, string> items, bool skip = false)
		{
			var sb = new StringBuilder();

			if (!skip)
			{
				sb.Append("?");
			}

			int count = 0;

			foreach (KeyValuePair<string, string> item in items)
			{
				sb.Append($"{item.Key}={item.Value}");

				if (count < items.Count - 1)
				{
					sb.Append("&");
				}

				count++;
			}

			return sb.ToString();
		}
	}
}