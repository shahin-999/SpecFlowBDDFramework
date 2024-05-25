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
			string rootPath = HelperUtility.GetInstance().GetProjectRootPath();
			string e = EnvReader.GetInstance().GetEnvType();
			Console.WriteLine(e);
			Console.WriteLine(ConfigReader.GetInstance().GetBaseUrl());
			Console.WriteLine(ConfigReader.GetInstance().GetDriverType());
			Console.WriteLine(ConfigReader.GetInstance().IsFullScreen());
			Console.WriteLine(ConfigReader.GetInstance().IsHeadless());
			Console.WriteLine(ConfigReader.GetInstance().ImplicitWait());

			string excelPath = Path.Combine(rootPath, "TestData\\New XLSX Worksheet.xlsx");
			ExcelReader excelReader = new ExcelReader(excelPath);
			int rowC = excelReader.GetRowCount("Sheet1");
			List<string> rowAll = new List<string>();
			rowAll = excelReader.ReadRow("Sheet1", 2);
			string cellData = excelReader.ReadCellData("Sheet1", 2, 3);


			Console.WriteLine($"Row count: {rowC}");
			Console.WriteLine($"Row all: {rowAll.Count}");
			Console.WriteLine($"Cell data: {cellData}");
			
			
			string jsonFilePath = "TestData\\TestData.json";
			
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
