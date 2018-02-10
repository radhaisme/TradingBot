namespace TradingBot.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPairInfos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PairInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UpdatedDt = c.DateTime(nullable: false),
                        AccountType = c.Int(nullable: false),
                        DecimalPlaces = c.Byte(nullable: false),
                        MinPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsHidden = c.Boolean(nullable: false),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FeeBuyer = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FeeSeller = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PairInfoes");
        }
    }
}
