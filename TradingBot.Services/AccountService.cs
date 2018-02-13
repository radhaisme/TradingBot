using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TradingBot.Common;
using TradingBot.Core.Enums;
using TradingBot.Data.Entities;

namespace TradingBot.Services
{
    public class AccountService: BaseService
    {
        public AccountService()
        {
        }

        public Account GetById(int accountId)
        {
            return UnitOfWork.Accounts.Query().FirstOrDefault(m => m.Id == accountId);
        }

        public Account GetByName(int userId, string accountName)
        {
            accountName = (accountName ?? "").Trim().ToLowerInvariant();
            return UnitOfWork.Accounts.Query().FirstOrDefault(m => m.Name.ToLower() == accountName);
        }

        public List<Account> GetAccounts(int userId)
        {
            return UnitOfWork.Accounts.Query().Where(m => m.UserId == userId).ToList();
        }

        public Account CreateOrUpdate(int userId, string name, AccountType type, string apiKey, string jsonSettings, int? id = null)
        {
            var item = GetById(id ?? 0);
            if (item == null)
            {
                item = new Account
                {
                    ApiKey = apiKey,
                    Name = name,
                    Type = type,
                    UserId = userId,
                    Settings = jsonSettings
                };
                UnitOfWork.Accounts.Add(item);
            }
            else
            {
                item.ApiKey = apiKey;
                item.Settings = jsonSettings;
                item.Name = name;
                item.Type = type;
                UnitOfWork.Accounts.Update(item);
            }

            UnitOfWork.SaveChanges();

            return item;
        }

        public Account UpdateSettings(int accountId, string jsonSettings)
        {
            var item = GetById(accountId);
            if (item != null)
            {
                item.Settings = jsonSettings;
                UnitOfWork.Accounts.Update(item);
                UnitOfWork.SaveChanges();
            }
            return item;
        }

    }
}