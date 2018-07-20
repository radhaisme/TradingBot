using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using TradingBot.Core.Enums;

namespace TradingBot.Core
{
	public abstract class ApiSettings : IApiSettings
	{
		public ExchangeType Type { get; set; }
		public string PublicUrl { get; set; }
		public string PrivateUrl { get; set; }
		public string ApiKey { get; set; }
		public string Secret { get; set; }
		public DateTimeOffset CreatedAt { get; set; }

		protected ApiSettings(Type type)
		{
			if (!type.IsInterface)
			{
				return;
			}

			Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(type).ManifestModule.Name);
			var interfaces = new List<Type>(2);
			interfaces.AddRange(type.GetInterfaces());
			interfaces.Add(type);
			IEnumerable<PropertyInfo> keys = interfaces.SelectMany(x => x.GetProperties());

			foreach (PropertyInfo key in keys)
			{
				if (!configuration.AppSettings.Settings.AllKeys.Contains(key.Name))
				{
					continue;
				}

				KeyValueConfigurationElement element = configuration.AppSettings.Settings[key.Name];
				PropertyInfo property = GetType().GetProperty(key.Name);
				object value = null;

				if (key.PropertyType.IsEnum)
				{
					value = Enum.Parse(typeof(ExchangeType), element.Value);
				}

				if (key.PropertyType == typeof(DateTimeOffset))
				{
					value = DateTimeOffset.Parse(element.Value);
				}
				else if (value == null)
				{
					value = Convert.ChangeType(element.Value, key.PropertyType);
				}

				property.SetValue(this, value);
			}
		}
	}
}