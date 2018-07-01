using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TradingBot.Data.Entities;

namespace TradingBot.Data
{
	public interface IRepository<TEntity> where TEntity : Entity
	{
		Task AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
		Task RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
		Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expr = null, CancellationToken cancellationToken = default(CancellationToken));
		Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
		Task<TEntity> FindAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
	}
}