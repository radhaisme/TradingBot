using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TradingBot.Domain.Base;

namespace TradingBot.Domain
{
    public class PairService: BaseService
    {
        public PairService()
        {
        }
   
        public PairInfo GetPair(AccountTypeEnum type, string tickerCode)
        {
            tickerCode = (tickerCode ?? "").Trim();
            var key = tickerCode.ToLowerInvariant();
            var item = dbContext.PairInfos.FirstOrDefault(m => m.AccountType == type && m.Name == key);
            return item;
        }

        public PairInfo AddOrUpdate(PairInfo info)
        {
            var name = (info.Name ?? "").Trim().ToLowerInvariant();

            var item = GetPair(info.AccountType, name);
            if (item != null)
            {
                item.DecimalPlaces = info.DecimalPlaces;
                item.Fee = info.Fee;
                item.FeeBuyer = info.FeeBuyer;
                item.FeeSeller = info.FeeSeller;
                item.IsHidden = info.IsHidden;
                item.MaxPrice = info.MaxPrice;
                item.MinAmount = info.MinAmount;
                item.MinPrice = info.MinPrice;
                item.MinTotal = info.MinTotal;
                item.UpdatedDt = info.UpdatedDt;
            }
            else
            {
                item = dbContext.PairInfos.Add(new PairInfo
                {
                    Name = name,
                    AccountType = info.AccountType,
                    DecimalPlaces = info.DecimalPlaces,
                    Fee = info.Fee,
                    FeeBuyer = info.FeeBuyer,
                    FeeSeller = info.FeeSeller,
                    IsHidden = info.IsHidden,
                    MaxPrice = info.MaxPrice,
                    MinAmount = info.MinAmount,
                    MinPrice = info.MinPrice,
                    MinTotal = info.MinTotal,
                    UpdatedDt = info.UpdatedDt
                });
            }
            dbContext.SaveChanges();

            return item;
        }

       
    }
}