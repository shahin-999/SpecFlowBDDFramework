using System.Text.RegularExpressions;

namespace SpecFlowBDDFramework.Utility
{
	public class HelperUtility
	{
		private static HelperUtility? helperUtility;
		public static HelperUtility GetInstance()
		{
			if (helperUtility == null)
			{
				helperUtility = new HelperUtility();
			}
			return helperUtility;
		}

		public string GetProjectRootPath()
		{
			//string rootPath = Regex.Replace(Environment.CurrentDirectory, "\\\\bin.*", "");
			string rootPath = Regex.Replace(AppDomain.CurrentDomain.BaseDirectory, "\\\\bin.*", "");
			return rootPath;
		}


	}
}
