using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using excel = Microsoft.Office.Interop.Excel;

namespace SpecFlowBDDFramework.Utility.DataProvider
{
	public class ExcelReader
	{
		excel.Application e = null;
		excel.Application excelApp = null;
		excel.Workbooks workbooks = null;
		excel.Workbook workbook = null;
		Hashtable sheets;
		public string excelFilePath;
		public ExcelReader(string excelFilePath) 
		{
			 this.excelFilePath = excelFilePath;
		}

		#region Open and Close Excel

		*//* Open Excel *//*
		public void OpenExcel()
		{
			excelApp = new excel.Application();
			workbooks = excelApp.Workbooks;
			workbook = workbooks.Open(excelFilePath);
			*//* Creating Hashtable *//*
			int count = 1;
            foreach (excel.Worksheet sheet in workbook.Sheets) 
            {
				sheets[count] = sheet.Name;
				count++;
            }
        }

		*//* Close Excel *//*
		public void CloseExcel()
		{
			workbook.Close(false, excelFilePath, null);
			Marshal.FinalReleaseComObject(workbook);
			workbook = null;

			workbooks.Close();
			Marshal.FinalReleaseComObject(workbooks);
			workbooks = null;

			excelApp.Quit();
			Marshal.FinalReleaseComObject(excelApp);
			excelApp = null;
		}

		#endregion

		#region Get Excel Sheet Row Count
		public int GetRowCount(string sheetName)
		{
			OpenExcel();
			//string value = string.Empty;
			int rowCount = 0;
			int sheetValue = 0;
			sheets = new Hashtable();

            if (sheets.ContainsValue(sheetName))
            {
                foreach (DictionaryEntry sheet in sheets)
                {
                    if (sheet.Value.Equals(sheetName))
                    {
						sheetValue = (int)sheet.Key;
                    }
                }
				// Getting particular worksheet using index/key from workbook
				excel.Worksheet worksheet = workbook.Worksheets[sheetValue] as excel.Worksheet;
				excel.Range range = worksheet.UsedRange; //Range of cells which having content
				rowCount = range.Rows.Count;
            }

			CloseExcel();

            return rowCount;
		}
		#endregion

	}
}