
namespace TradingBot.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Entities;
	using Microsoft.EntityFrameworkCore;

	public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
	{
		private readonly IDataContext _context;
		protected DbSet<TEntity> Set;

		public Repository(IDataContext context)
		{
			_context = context;
			Set = context.Set<TEntity>();			
		}

		public void Add(TEntity entity)
		{
			Set.Add(entity);
		}

		public void Delete(int id)
		{
			TEntity entity = Find(id);

			if (entity == null)
			{
				return;
			}

			Set.Remove(entity);
		}

		public ICollection<TEntity> Get(Expression<Func<TEntity, bool>> expr = null)
		{
			if (expr != null)
			{
				return Set.Where(expr).ToList();
			}

			return Set.ToList();
		}

		public void Update(TEntity entity)
		{
			Set.Update(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}

		public IQueryable<TEntity> Query()
		{
			return Set;
		}

		public TEntity Find(int id)
		{
			return Set.Find(id);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_context.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}