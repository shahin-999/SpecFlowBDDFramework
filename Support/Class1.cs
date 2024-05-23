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
			/*ExcelReader_EPPlus excelReader = new ExcelReader_EPPlus(excelPath);
			Console.WriteLine($"Row count sheet1 = {excelReader.GetRowCount("Sheet1")}");
			Console.WriteLine($"Row count sheet2 = {excelReader.GetRowCount("Sheet2")}");
			Console.WriteLine($"Last row number sheet1 = {excelReader.GetLastRowNumber("Sheet1")}");
			Console.WriteLine($"Last row number sheet2 = {excelReader.GetLastRowNumber("Sheet2")}");

			string a = excelReader.ReadCellData("Sheet1",2, "Column-33");
			Console.WriteLine(a);
			a = excelReader.ReadCellData("Sheet1", 2, 200);
			Console.WriteLine(a);*/

			ExcelReader excelReader = new ExcelReader(excelPath);
			Console.WriteLine($"Row count sheet1 = {excelReader.GetRowCount("Sheet1")}");
			Console.WriteLine($"Row count sheet2 = {excelReader.GetRowCount("Sheet2")}");
			/*Console.WriteLine($"Last row number sheet1 = {excelReader.GetRowCount("Sheet1")}");
			Console.WriteLine($"Last row number sheet2 = {excelReader.GetLastRowNumber("Sheet2")}");*/

			string a = excelReader.ReadCell( 2, 5, "Sheet1");
			Console.WriteLine($"A1: {a}");
			a = excelReader.ReadCell(2, 5, "Sheet1");
			Console.WriteLine($"2:4 of s1 {a}");

			//List<string> list = new List<string> { "Shahin", "Afrin"};
			//excelReader.CreateSheet("Hello1", list);
			//Console.WriteLine(excelReader.ReadCellData("Hello1", 1, 2));

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
