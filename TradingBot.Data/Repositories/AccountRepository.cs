
namespace TradingBot.Data.Repositories
{
	using Contracts;
	using Entities;
	using Enums;
	using System;
	using System.Collections.Generic;

	public class AccountRepository : Repository<Account>, IAccountRepository
	{
	    public AccountRepository(IDataContext context) : base(context)
	    { }

		public void CreateAccount() //Just for test purpose
		{
			var account = new Account
			{
				ApiKey = Guid.NewGuid().ToString(),
				Secret = Guid.NewGuid().ToString(),
				Name = "Bulko-" + Guid.NewGuid(),
				Type = AccountType.Yobit
			};

			Add(account);
		}

		public ICollection<Account> GetAllAccounts() //Just for test purpose
		{
			return Get();
		}
    }
}