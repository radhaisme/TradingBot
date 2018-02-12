﻿
namespace TradingBot.Services
{
	using Domain;
	using System.Linq;
	using Core;

	public class PairService: BaseService
    {
        public PairService()
        {
        }
   
        public PairInfo GetPair(AccountTypeEnum type, string tickerCode)
        {
            tickerCode = (tickerCode ?? "").Trim();
            var key = tickerCode.ToLowerInvariant();
            var item = DbContext.PairInfos.FirstOrDefault(m => m.AccountType == type && m.Name == key);
            return item;
        }

        public PairInfo AddOrUpdate(PairInfo info, bool commit = true)
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
                item = DbContext.PairInfos.Add(new PairInfo
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
            if(commit)
                DbContext.SaveChanges();

            return item;
        }

        public PairsResponse<T> PullPairs<T>(ExchangeApi api)
        {
            var result = api.GetPairs<T>();
            if (result.IsSuccess)
            {
                switch (api.Type)
                {
                    //case AccountType.Yobit:
                    //    {
                    //        var data = result.Data as Dictionary<string, Pair>;
                    //        foreach (var pair in data)
                    //        {
                    //            var pairInfo = pair.Value;
                    //            AddOrUpdate(new PairInfo
                    //            {
                    //                AccountType = api.Type,
                    //                UpdatedDt = DateTime.UtcNow,
                    //                Name = pair.Key,
                    //                DecimalPlaces = pairInfo.DecimalPlaces,
                    //                Fee = pairInfo.Fee,
                    //                FeeBuyer = pairInfo.FeeBuyer,
                    //                FeeSeller = pairInfo.FeeSeller,
                    //                IsHidden = pairInfo.IsHidden,
                    //                MaxPrice = pairInfo.MaxPrice,
                    //                MinAmount = pairInfo.MinAmount,
                    //                MinPrice = pairInfo.MinPrice,
                    //                MinTotal = pairInfo.MinTotal
                    //            });
                    //        }
                    //        DbContext.SaveChanges();
                    //    }
                    //    break;
                    //default:
                    //    break;

                }
            }
            return result;
        }

    }
}