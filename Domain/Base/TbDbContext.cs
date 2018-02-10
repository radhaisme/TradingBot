using System.Data.Entity;

namespace TradingBot.Domain.Base
{
    public class TbDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public TbDbContext()
            : base("DbConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .Property(u => u.DisplayName)
            //    .HasColumnName("display_name");
        }
    }
}
