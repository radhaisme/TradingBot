
namespace TradingBot.Data
{
    using TradingBot.Data.Entities;

    public class UnitOfWork : IUnitOfWork
    {
	    private readonly IDataContext _context;

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Repository<Account> Accounts { get; private set; }

        public Repository<User> Users { get; private set; }

        public Repository<PairInfo> PairInfos { get; private set; }

        public UnitOfWork() //Add IoC
		{
			var context = new BotContext();
			context.Database.EnsureCreated();
			_context = context;

            Accounts = new Repository<Account>(_context); //Add IoC
            Users = new Repository<User>(_context); //Add IoC
            PairInfos = new Repository<PairInfo>(_context); //Add IoC
        }

	}
}