using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TradingBot.Common;
using TradingBot.Core;

namespace TradingBot.Tasks
{
	public class SomeTask : ScopedService
	{
		public SomeTask(IServiceScopeFactory factory) : base(factory)
		{ }

		public override Task ProcessInScope(IServiceProvider serviceProvider)
		{
			var logger = serviceProvider.GetService<ILog>();

			return null;
		}
	}
}