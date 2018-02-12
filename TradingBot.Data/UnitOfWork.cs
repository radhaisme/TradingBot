
namespace TradingBot.Data
{
	using Repositories;
	using Repositories.Contracts;

	public class UnitOfWork : IUnitOfWork
    {
	    private readonly IDataContext _context;

		public UnitOfWork(IDataContext context) //Add IoC
		{
			_context = new BotContext();
		}

	    public IAccountRepository Accounts
	    {
		    get
		    {
				return new AccountRepository(_context); //Add IoC
		    }
	    }

	    public int SaveChanges()
		{
			return _context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}