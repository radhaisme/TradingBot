
namespace TradingBot.Data.Entities
{
	using Common;
	using Core.Enums;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class YobitSettings
    {
        public string Secret { get; set; }
        public int Counter { get; set; }
    }

    public class Account : Entity
	{
        [Required]
        public string Name { get; set; }

        [Required]
        public AccountType Type { get; set; }

        public string ApiKey { get; set; }

        public string Settings { get; set; }

        public int UserId { get; set; }

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