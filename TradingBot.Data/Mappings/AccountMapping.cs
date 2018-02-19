
namespace TradingBot.Data.Mappings
{
	using Entities;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	public class AccountMapping : IEntityTypeConfiguration<Account>
	{
		public void Configure(EntityTypeBuilder<Account> builder)
		{
			builder.ToTable("Accounts");
			builder.HasKey(_ => _.Id);
			builder.Property(_ => _.Name).IsRequired().HasMaxLength(128);
			builder.Property(_ => _.ExchangeType).IsRequired();
			builder.Property(_ => _.UserId).IsRequired();
			builder.Property(_ => _.ApiSettings).IsRequired(false);
		}
	}
}