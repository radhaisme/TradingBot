
namespace TradingBot.Data.Repositories.Contracts
{
	using Entities;
	using System.Collections.Generic;

	public interface IAccountRepository
    {
	    void CreateAccount();
	    ICollection<Account> GetAllAccounts();
    }
}