
namespace TradingBot.Data.Mappings
{
	using Entities;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	public class UserMapping : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");
			builder.HasKey(_ => _.Id);
			builder.Property(_ => _.Username).IsRequired().HasMaxLength(128);
			builder.Property(_ => _.PasswordHash).IsRequired().HasMaxLength(1024);
			builder.Property(_ => _.PasswordSalt).IsRequired(false).HasMaxLength(1024);
		}
	}
}