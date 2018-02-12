
namespace TradingBot.Data
{
	using System;

	public interface IUnitOfWork : IDisposable
	{
	    int SaveChanges();
    }
}