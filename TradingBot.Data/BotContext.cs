
namespace TradingBot.Data
{
	using Entities;
	using Microsoft.EntityFrameworkCore;
	using System.Configuration;
	using Mappings;

	public class BotContext : DbContext, IDataContext
	{
		public DbSet<Account> Accounts { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			//ChangeTracker.AutoDetectChangesEnabled = false;
			options.UseSqlite(ConfigurationManager.ConnectionStrings["BotConnectionString"].ConnectionString);
		}

		protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMapping());
            builder.ApplyConfiguration(new AccountMapping());
            builder.ApplyConfiguration(new PairInfoMapping());
        }
	}
}