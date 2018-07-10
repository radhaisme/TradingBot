using System.Threading;
using System.Threading.Tasks;
using TradingBot.Data.Entities;

namespace TradingBot.Data
{


	public sealed class UnitOfWork : IUnitOfWork
	{
		private readonly IDataContext _context;

		public UnitOfWork() //Add IoC
		{
			var context = new BotContext();
			context.Database.EnsureCreated();
			_context = context;

			//Accounts = new Repository<Account>(_context); //Add IoC
			//Users = new Repository<User>(_context); //Add IoC
			//PairInfos = new Repository<PairInfo>(_context); //Add IoC
		}

		public Repository<Account> Accounts { get; private set; }

		public Repository<User> Users { get; private set; }

		//public Repository<PairInfo> PairInfos { get; private set; }

		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			return await _context.SaveChangesAsync(cancellationToken);
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}