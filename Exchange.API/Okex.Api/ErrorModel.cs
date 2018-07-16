using System;
using Newtonsoft.Json;

namespace Okex.Api
{
	internal class ErrorModel
	{
		[JsonProperty("error_code")]
		public int ErrorCode { get; set; }

		public string GetMessage()
		{
			switch (ErrorCode)
			{
				case 1007:
					{
						return "No trading market information";
					}
				default:
					{
						return String.Empty;
					}
			}
		}
	}
}