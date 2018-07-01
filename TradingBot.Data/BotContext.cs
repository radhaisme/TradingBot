using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace TradingBot.Data
{
	public class BotContext : DbContext, IDataContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			//ChangeTracker.AutoDetectChangesEnabled = false;
			options.UseSqlite(ConfigurationManager.ConnectionStrings["BotConnectionString"].ConnectionString);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			
			
			//builder.ApplyConfiguration(new UserMapping());
			//builder.ApplyConfiguration(new AccountMapping());
			//builder.ApplyConfiguration(new PairInfoMapping());
			//builder.ApplyConfiguration(new ExchangeClientMapping());
		}
	}
}