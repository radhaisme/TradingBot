
using Newtonsoft.Json;

namespace Yobit.Exchange.Api.Entities
{
    public class Pair
    {
        [JsonProperty(PropertyName = "decimal_places")]
        public byte DecimalPlaces { get; set; }

        [JsonProperty(PropertyName = "min_price")]
        public decimal MinPrice { get; set; }

        [JsonProperty(PropertyName = "max_price")]
        public decimal MaxPrice { get; set; }

        [JsonProperty(PropertyName = "min_amount")]
        public decimal MinAmount { get; set; }

        [JsonProperty(PropertyName = "min_total")]
        public decimal MinTotal { get; set; }

        [JsonProperty(PropertyName = "hidden")]
        public bool IsHidden { get; set; }

        [JsonProperty(PropertyName = "fee")]
        public decimal Fee { get; set; }

        [JsonProperty(PropertyName = "fee_buyer")]
        public decimal FeeBuyer { get; set; }

        [JsonProperty(PropertyName = "fee_seller")]
        public decimal FeeSeller { get; set; }

    }
}