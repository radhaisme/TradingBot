﻿

namespace TradingBot.Services
{
	using Core.Enums;
	using Data.Entities;
	using System;
	using System.Linq;
	using System.Reflection;
	using Core;
	using Yobit.Api;

	public class PairService : BaseService
    {
        public PairService()
        {
        }

        public PairInfo GetPair(AccountType type, string tickerCode)
        {
            tickerCode = (tickerCode ?? "").Trim();
            var key = tickerCode.ToLowerInvariant();
            var item = UnitOfWork.PairInfos.Query().FirstOrDefault(m => m.AccountType == type && m.Name == key);
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
                item = new PairInfo
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
                };
                UnitOfWork.PairInfos.Add(item);
            }
            if (commit)
                UnitOfWork.SaveChanges();

            return item;
        }

        public dynamic PullPairs(ExchangeApi api)
        {
            Type type = null;

            switch (api.Type)
            {
                case AccountType.Yobit:
                    type = typeof(YobitPairsResponse);
                    break;
                default:
                    break;
            }

            if (type == null)
                return new BasePairsResponse("Method is not implemented");

            var method = typeof(ExchangeApi).GetMethod("GetPairs");
            var generic = method.MakeGenericMethod(type.BaseType.GenericTypeArguments[0].GetTypeInfo());
            dynamic result = generic.Invoke(api, null);

            if (result.IsSuccess)
            {
                switch (api.Type)
                {
                    case AccountType.Yobit:
                        {
                            var data = result as YobitPairsResponse;
                            foreach (var pair in data.Data)
                            {
                                var pairInfo = pair.Value;
                                AddOrUpdate(new PairInfo
                                {
                                    AccountType = api.Type,
                                    UpdatedDt = DateTime.UtcNow,
                                    Name = pair.Key,
                                    DecimalPlaces = pairInfo.DecimalPlaces,
                                    Fee = pairInfo.Fee,
                                    FeeBuyer = pairInfo.FeeBuyer,
                                    FeeSeller = pairInfo.FeeSeller,
                                    IsHidden = pairInfo.IsHidden,
                                    MaxPrice = pairInfo.MaxPrice,
                                    MinAmount = pairInfo.MinAmount,
                                    MinPrice = pairInfo.MinPrice,
                                    MinTotal = pairInfo.MinTotal
                                });
                            }
                            UnitOfWork.SaveChanges();
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}