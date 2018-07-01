
namespace TradingBot.Services
{
	public class AccountService : BaseService
	{
		//public Account GetById(int id)
		//{
		//	return Context.Accounts.FindAsync(id);
		//}

		//public Account GetByName(int userId, string name)
		//{
  //          var accountName = (name ?? "").Trim().ToLowerInvariant();

		//	return Context.Accounts.Query().FirstOrDefault(m => m.UserId == userId && m.Name.ToLower() == accountName); 
		//}

		//public List<Account> GetAccounts(int userId)
		//{
		//	return Context.Accounts.GetAsync(x => x.UserId == userId).ToList();
		//}

		//public Account CreateOrUpdate(int userId, string name, ExchangeType exchangeType, string jsonSettings, int? id = null)
		//{
		//	var account = id.HasValue ? GetById(id.Value) : new Account();
		//	account.ApiSettings = jsonSettings;
  //          account.Name = (name ?? "").Trim();
		//	account.ExchangeType = exchangeType;

		//	if (account.IsNew())
		//	{
		//		account.UserId = userId;
		//		Context.Accounts.Add(account);
		//	}
		//	else
		//	{
		//		Context.Accounts.UpdateAsync(account);
		//	}

		//	Context.SaveChanges();

		//	return account;
		//}

		//public Account UpdateSettings(int accountId, string jsonSettings)
		//{
		//	Account account = GetById(accountId);

		//	if (account != null)
		//	{
		//		account.ApiSettings = jsonSettings;
		//		Context.Accounts.UpdateAsync(account);
		//		Context.SaveChanges();
		//	}

		//	return account;
		//}
	}
}