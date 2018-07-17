using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace TradingBot.Core
{
	public abstract class ScopedService : BackgroundService
	{
		private readonly IServiceScopeFactory _factory;

		protected ScopedService(IServiceScopeFactory factory)
		{
			_factory = factory;
		}

		protected override async Task Process()
		{
			using (var scope = _factory.CreateScope())
			{
				await ProcessInScope(scope.ServiceProvider);
			}
		}

		public abstract Task ProcessInScope(IServiceProvider serviceProvider);
	}
}