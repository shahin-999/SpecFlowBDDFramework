using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpecFlowBDDFramework.Utility.PropertyReader;

namespace SpecFlowBDDFramework.Utility
{
	public enum DriverType
	{
		CHROME,
		FIREFOX,
		EDGE,
		SAFARI
	}

	public class ConfigReader
	{
		private readonly Dictionary<string, string> properties;
		private static ConfigReader configReader;
		//private string env = EnvReader.GetInstance().GetEnvironment();
		private string env = EnvReader.GetInstance().GetEnvType();

		private ConfigReader()
		{
			properties = new Dictionary<string, string>();
			string rootPath = HelperUtility.GetInstance().GetProjectRootPath();
			string propertyFilePath = Path.Combine(rootPath, $"AppConfig\\Config\\{env}.properties");

			try
			{
				using (var reader = new StreamReader(propertyFilePath))
				{
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						var keyValue = line.Split('=');
						if (keyValue.Length == 2)
						{
							properties[keyValue[0].Trim()] = keyValue[1].Trim();
						}
					}
				}
			}
			catch (FileNotFoundException e)
			{
				throw new Exception($"Configuration.properties not found at {propertyFilePath}", e);
			}
			catch (IOException e)
			{
				throw new Exception($"Error reading properties file at {propertyFilePath}", e);
			}
		}

		public static ConfigReader GetInstance()
		{
			if (configReader == null)
			{
				configReader = new ConfigReader();
			}
			return configReader;
		}

		public string GetAnyPropValue(string keyName)
		{
			if (properties.TryGetValue(keyName, out var value))
			{
				return value;
			}
			else
			{
				throw new Exception($"{keyName} not specified in the {env}.properties file.");
			}
		}

		public string GetBaseUrl()
		{
			if (properties.TryGetValue("baseURL", out var baseUrl))
			{
				return baseUrl;
			}
			else
			{
				throw new Exception($"Base Url not specified in the {env}.properties file.");
			}
		}

		public DriverType GetDriverType()
		{
			if (properties.TryGetValue("browser", out var browser))
			{
				switch (browser.ToLower())
				{
					case "edge":
						return DriverType.EDGE;
					case "firefox":
						return DriverType.FIREFOX;
					case "safari":
						return DriverType.SAFARI;
					default:
						return DriverType.CHROME;
				}
			}
			else
			{
				return DriverType.CHROME;
			}
		}

		public bool IsFullScreen()
		{
			if (properties.TryGetValue("fullScreen", out var isFull))
			{
				return bool.TryParse(isFull, out var result) && result;
			}
			return true;
		}

		public bool IsHeadless()
		{
			if (properties.TryGetValue("headless", out var isFull))
			{
				return bool.TryParse(isFull, out var result) && result;
			}
			return true;
		}

		public int ImplicitWait()
		{
			if (properties.TryGetValue("implicitWait", out var implicitWait))
			{
				if (int.TryParse(implicitWait, out var waitTime))
				{
					return waitTime;
				}
			}
			return 30;
		}
	}

}
