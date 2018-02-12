
namespace TradingBot.Data
{
    using TradingBot.Data.Entities;

    public class UnitOfWork : IUnitOfWork
    {
	    private readonly IDataContext _context;

		public UnitOfWork() //Add IoC
		{
			var ctx = new BotContext();
			ctx.Database.EnsureCreated();
			_context = ctx;
		}

	    public Repository<Account> Accounts
	    {
		    get
		    {
				return new Repository<Account>(_context); //Add IoC
		    }
	    }

        public Repository<User> Users
        {
            get
            {
                return new Repository<User>(_context); //Add IoC
            }
        }

        public Repository<PairInfo> PairInfos
        {
            get
            {
                return new Repository<PairInfo>(_context); //Add IoC
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