using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Bitfinex.Api.Models
{
	public abstract class OrderRequest
	{
		protected OrderRequest()
		{
			Nonce = DateTime.Now.ToString(CultureInfo.InvariantCulture);
		}

		[JsonProperty(PropertyName = "nonce")]
		public string Nonce { get; }
	}
}