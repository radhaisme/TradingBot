
namespace TradingBot.Data.Mappings
{
	using Entities;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	public class PairInfoMapping : IEntityTypeConfiguration<PairInfo>
	{
		public void Configure(EntityTypeBuilder<PairInfo> builder)
		{
			builder.ToTable("PairInfos");
			builder.HasKey(_ => _.Id);
			builder.Property(_ => _.Name).IsRequired().HasMaxLength(128);
			builder.Property(_ => _.AccountType).IsRequired();
			builder.Property(_ => _.DecimalPlaces).IsRequired(false);
			builder.Property(_ => _.Fee).IsRequired(false);
            builder.Property(_ => _.FeeBuyer).IsRequired(false);
            builder.Property(_ => _.FeeSeller).IsRequired(false);
            builder.Property(_ => _.IsHidden).IsRequired(false);
            builder.Property(_ => _.MaxPrice).IsRequired(false);
            builder.Property(_ => _.MinAmount).IsRequired(false);
            builder.Property(_ => _.MinPrice).IsRequired(false);
            builder.Property(_ => _.MinTotal).IsRequired(false);
            builder.Property(_ => _.UpdatedDt).IsRequired(false);
        }
	}
}