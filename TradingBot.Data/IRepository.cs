
namespace TradingBot.Data
{
	using Entities;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	public interface IRepository<TEntity> : IDisposable where TEntity : Entity
	{
		void Add(TEntity entity);
	    void Delete(int id);
	    ICollection<TEntity> Get(Expression<Func<TEntity, bool>> expr = null);
	    void Update(TEntity entity);
	    IQueryable<TEntity> Query();
		TEntity Find(int id);
	}
}