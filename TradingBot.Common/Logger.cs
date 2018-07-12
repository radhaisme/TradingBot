using NLog;
using NLog.Web;

namespace TradingBot.Common
{
	public class Logger : ILog
	{
		private readonly ILogger _logger;

		public Logger()
		{
			_logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
		}

		public void Trace(string message)
		{
			_logger.Trace(message);
		}

		public void Debug(string message)
		{
			_logger.Debug(message);
		}

		public void Error(string message)
		{
			_logger.Error(message);
		}

		public void Fatal(string message)
		{
			_logger.Fatal(message);
		}

		public void Info(string message)
		{
			_logger.Info(message);
		}

		public void Warn(string message)
		{
			_logger.Warn(message);
		}
	}
}