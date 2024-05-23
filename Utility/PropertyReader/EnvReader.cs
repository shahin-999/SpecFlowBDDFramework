using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlowBDDFramework.Utility.PropertyReader
{
	public enum EnvType
	{
		LOCAL,
		DEV,
		STAGE,
		UAT
	}
	public class EnvReader
	{
		private readonly Dictionary<string, string> properties;
		private static EnvReader envReader;

		private EnvReader()
		{
			properties = new Dictionary<string, string>();
			string rootPath = HelperUtility.GetInstance().GetProjectRootPath();
			string envPropertyFilePath = Path.Combine(rootPath, "AppConfig\\Env\\env.properties");

			try
			{
				using (var reader = new StreamReader(envPropertyFilePath))
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
				throw new Exception($"Configuration.properties not found at {envPropertyFilePath}", e);
			}
			catch (IOException e)
			{
				throw new Exception($"Error reading properties file at {envPropertyFilePath}", e);
			}
		}

		public string GetAnyPropValue(string keyName)
		{
			if (properties.TryGetValue(keyName, out var value))
			{
				return value;
			}
			else
			{
				throw new Exception($"{keyName} not specified in the env.properties file.");
			}
		}

		public string GetEnvType()
		{
			if (properties.TryGetValue("env", out var env))
			{
				switch (env.ToLower())
				{
					case "local":
						return EnvType.LOCAL.ToString().ToLower();
					case "stage":
						return EnvType.STAGE.ToString().ToLower();
					case "uat":
						return EnvType.UAT.ToString().ToLower();
					default:
						return EnvType.DEV.ToString().ToLower();
				}
			}
			else
			{
				return EnvType.DEV.ToString().ToLower();
			}
		}

		public string GetOS()
		{
			if (properties.TryGetValue("OS", out var OS))
			{
				return OS;
			}
			else
			{
				throw new Exception($"OS name not specified in the env.properties file.");
			}
		}

		public string GetAppName()
		{
			if (properties.TryGetValue("appName", out var appName))
			{
				return appName;
			}
			else
			{
				throw new Exception($"Application name not specified in the env.properties file.");
			}
		}

		public static EnvReader GetInstance()
		{
			if (envReader == null)
			{
				envReader = new EnvReader();
			}
			return envReader;
		}
	}
}
