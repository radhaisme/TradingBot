
namespace TradingBot.Data.Mappings
{
	using Entities;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	public class ExchangeClientMapping : IEntityTypeConfiguration<ExchangeClient>
	{
		public void Configure(EntityTypeBuilder<ExchangeClient> builder)
		{
			builder.HasKey(_ => _.Id);
			builder.Property(_ => _.Path).IsRequired().HasMaxLength(255);
			builder.Property(_ => _.FullName).IsRequired().HasMaxLength(255);
			builder.Property(_ => _.ExchangeType).IsRequired();
		}
	}
}