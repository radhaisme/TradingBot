using System;
using System.Threading;
using System.Threading.Tasks;

namespace TradingBot.Data
{
	public interface IUnitOfWork : IDisposable
	{
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}