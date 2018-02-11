
namespace TradingBot.Data
{
	using Entities;
	using Microsoft.EntityFrameworkCore;
	using System.Configuration;

	public class BotContext : DbContext
	{
		public DbSet<Account> Accounts { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{


			optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["BotConnectionString"].ConnectionString);
		}
	}
}