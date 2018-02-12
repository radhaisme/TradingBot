
namespace TradingBot.Data.Entities
{
	using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User: Entity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

		public string PasswordSalt { get; set; }

		public virtual ICollection<Account> Accounts { get; set; }
    }
}