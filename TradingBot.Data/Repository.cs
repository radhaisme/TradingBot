using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TradingBot.Data.Entities;

namespace TradingBot.Data
{
	public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
	{
		private readonly IDataContext _context;
		private readonly DbSet<TEntity> _set;

		protected Repository(IDataContext context)
		{
			_context = context;
			_set = context.Set<TEntity>();
		}

		public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			await _set.AddAsync(entity, cancellationToken);
		}

		public async Task RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
		{
			TEntity entity = await FindAsync(id, cancellationToken);

			if (entity == null)
			{
				return;
			}

			_set.Remove(entity);
		}

		public async Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expr = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await Task.Factory.StartNew(() =>
			{
				if (expr != null)
				{
					return _set.Where(expr).ToList();
				}

				return _set.ToList();
			}, cancellationToken);
		}

		public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			await Task.Factory.StartNew(() =>
			{
				_set.Update(entity);
				_context.Entry(entity).State = EntityState.Modified;
			}, cancellationToken);
		}

		protected IQueryable<TEntity> Query()
		{
			return _set;
		}

		public async Task<TEntity> FindAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await _set.FindAsync(id, cancellationToken);
		}
	}
}