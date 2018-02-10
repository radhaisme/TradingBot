using Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingBot.Domain
{
    public class YobitSettings
    {

    }
    
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public AccountTypeEnum Type { get; set; }

        public string ApiKey { get; set; }

        public string Settings { get; set; }

        public virtual User User { get; set; }

        [NotMapped]
        public YobitSettings YobitSettings
        {
            get
            {
                return JsonHelper.ToObject<YobitSettings>(Settings);
            }
            set
            {
                Settings = JsonHelper.ToJson(value);
            }
        }
    }
}