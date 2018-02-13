
namespace TradingBot.Services
{
	using Data;
	using System;

	public abstract class BaseService : IDisposable
	{
		protected readonly UnitOfWork Context;

		protected BaseService()
		{
			Context = new UnitOfWork();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Context.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}