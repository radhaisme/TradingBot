using System;
using System.ComponentModel.DataAnnotations;
using TradingBot.Core.Enums;

namespace TradingBot.Data.Entities
{
    public class PairInfo: Entity
    {
        public string Name { get; set; }

        public DateTime UpdatedDt { get; set; }

        public AccountType AccountType { get; set; }

        public byte DecimalPlaces { get; set; }
        
        public decimal MinPrice { get; set; }
        
        public decimal MaxPrice { get; set; }
        
        public decimal MinAmount { get; set; }
        
        public decimal MinTotal { get; set; }
        
        public bool IsHidden { get; set; }
        
        public decimal Fee { get; set; }
        
        public decimal FeeBuyer { get; set; }
        
        public decimal FeeSeller { get; set; }

    }
}