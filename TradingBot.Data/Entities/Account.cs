
namespace TradingBot.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TradingBot.Common;
    using TradingBot.Core.Enums;

    public class YobitSettings
    {
        public string Secret { get; set; }
    }

    public class Account : Entity
	{
        [Required]
        public string Name { get; set; }

        [Required]
        public AccountType Type { get; set; }

        public string ApiKey { get; set; }

        public string Settings { get; set; }

        public virtual User User { get; set; }

        [NotMapped]
        public YobitSettings YobitSettings
        {
            get
            {
                return JsonHelper.FromJson<YobitSettings>(Settings);
            }
            set
            {
                Settings = JsonHelper.ToJson(value);
            }
        }
    }
}