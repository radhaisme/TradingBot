
namespace TradingBot.Data
{
	using Entities;
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
	{
		private readonly IDataContext _context;
		private readonly DbSet<TEntity> _set;

		public Repository(IDataContext context)
		{
			_context = context;
			_set = context.Set<TEntity>();
		}

		public void Add(TEntity entity)
		{
			_set.Add(entity);
		}

		public void Delete(int id)
		{
			TEntity entity = Find(id);

			if (entity == null)
			{
				return;
			}

			_set.Remove(entity);
		}

		public ICollection<TEntity> Get(Expression<Func<TEntity, bool>> expr = null)
		{
			if (expr != null)
			{
				return _set.Where(expr).ToList();
			}

			return _set.ToList();
		}

		public void Update(TEntity entity)
		{
			_set.Update(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}

		public IQueryable<TEntity> Query()
		{
			return _set;
		}

		public TEntity Find(int id)
		{
			return _set.Find(id);
		}
	}
}