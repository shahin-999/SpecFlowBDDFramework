using System.Text.RegularExpressions;
using NUnit.Framework;
using SpecFlowBDDFramework.Utility;
using SpecFlowBDDFramework.Utility.DataProvider;
using SpecFlowBDDFramework.Utility.PropertyReader;

namespace SpecFlowBDDFramework.Support
{

    public class Person
	{
		public string? Name { get; set; }
		public int Age { get; set; }
		public string? City { get; set; }
	}
	[Binding]
	public class Class1
	{
		[Test]
		public void DemoTest()
		{
			string e = EnvReader.GetInstance().GetEnvType();
			Console.WriteLine(e);
			Console.WriteLine(ConfigReader.GetInstance().GetBaseUrl());
			Console.WriteLine(ConfigReader.GetInstance().GetDriverType());
			Console.WriteLine(ConfigReader.GetInstance().IsFullScreen());
			Console.WriteLine(ConfigReader.GetInstance().IsHeadless());
			Console.WriteLine(ConfigReader.GetInstance().ImplicitWait());

			ExcelReader excelReader = new ExcelReader(@"C:/Users/BS981/OneDrive - Brain Station 23 Ltd/Learning/Selenium - CS/SpecFlowBDDFramework/TestData/New XLSX Worksheet.xlsx");
			Console.WriteLine($"Row number: {excelReader.GetRowCount("Sheet1")}");

			string jsonFilePath = "TestData\\TestData.json";
			string rootPath = HelperUtility.GetInstance().GetProjectRootPath();
			string _jsonFilePath = Path.Combine(rootPath, jsonFilePath);
			Console.WriteLine(_jsonFilePath);

			try
			{
				Person person = JSONFileReader.ReadJsonFile<Person>(_jsonFilePath);
				Console.WriteLine($"Name: {person.Name}");
				Console.WriteLine($"Age: {person.Age}");
				Console.WriteLine($"City: {person.City}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}
	}
}
