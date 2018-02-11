
namespace TradingBot.Data.Entities
{
	using System.Collections.Generic;

	public class User
    {
	    public int Id { get; set; }
	    public string Username { get; set; }
	    public string PasswordHash { get; set; }
		public string PasswordSalt { get; set; }
		public virtual ICollection<Account> Accounts { get; set; }
	}
}