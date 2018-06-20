using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace TradingBot.Core
{
	public abstract class ApiSettings
	{
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
				object value;
				
				if (key.PropertyType == typeof(DateTimeOffset))
				{
					value = DateTimeOffset.Parse(element.Value);
				}
				else
				{
					value = Convert.ChangeType(element.Value, key.PropertyType);
				}

				property.SetValue(this, value);
			}
		}
	}
}