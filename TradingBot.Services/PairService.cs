

namespace TradingBot.Services
{
	using Core.Enums;
	using Data.Entities;
	using System;
	using System.Linq;
	using System.Reflection;
	using Core;
	using Core.Entities;
	using Yobit.Api;

	public class PairService : BaseService
    {
        public PairService()
        {
        }

        public PairInfo GetPair(ExchangeType type, string tickerCode)
        {
            var key = (tickerCode ?? "").Trim().ToLowerInvariant();
            var item = Context.PairInfos.Query().FirstOrDefault(m => m.AccountType == type && m.Name == key);
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
                Context.PairInfos.Add(item);
            }
            if (commit)
                Context.SaveChanges();

            return item;
        }

        public dynamic PullPairs(object api)
        {
	        var method = api.GetType().GetMethod("GetPairs");
	        var result = (PairsInfo)method.Invoke(api, null);
			string[] names = Enum.GetNames(typeof(ExchangeType));
	        var name = names.Single(x => api.GetType().Name.StartsWith(x));
	        var type = (ExchangeType)Enum.Parse(typeof(ExchangeType), name);

			switch (type)
			{
				case ExchangeType.Yobit:
				{
					foreach (var pair in result.Pairs)
					{
						var pairInfo = pair.Value;
						AddOrUpdate(new PairInfo
						{
							AccountType = type,
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
						}, false);
					}
					Context.SaveChanges();
				}
					break;
				default:
					break;
			}
			
			return result;
        }
    }
}