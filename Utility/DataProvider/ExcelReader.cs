using OfficeOpenXml;

namespace SpecFlowBDDFramework.Utility.DataProvider
{
	public class ExcelReader
	{
		string filePath;
		public ExcelReader(string filePath)
		{
			this.filePath = filePath;
			// Ensure the EPPlus license context is set.
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}

		#region Get Row and Column Count
		public int GetRowCount(string sheetName)
		{
			using (var package = new ExcelPackage(new FileInfo(filePath)))
			{
				var worksheet = package.Workbook.Worksheets[sheetName];
				if (worksheet == null || worksheet.Dimension == null)
				{
					return 0; // No such worksheet or no data
				}
				return worksheet.Dimension.Rows;
			}
		}

		public int GetLastRowNumber(string sheetName)
		{
			using (var package = new ExcelPackage(new FileInfo(filePath)))
			{
				var worksheet = package.Workbook.Worksheets[sheetName];
				if (worksheet == null || worksheet.Dimension == null)
				{
					return 0; // No such worksheet or no data
				}
				return worksheet.Dimension.End.Row;
			}
		}

		public int GetColumnCount(string sheetName)
		{
			using (var package = new ExcelPackage(new FileInfo(filePath)))
			{
				var worksheet = package.Workbook.Worksheets[sheetName];
				if (worksheet == null || worksheet.Dimension == null)
				{
					return 0; // No such worksheet or no data
				}

				return worksheet.Dimension.Columns;
			}
		}

		#endregion

		#region Read Cell Data
		public string ReadCellData(string sheetName, int row, int column)
		{
			using (var package = new ExcelPackage(new FileInfo(filePath)))
			{
				var worksheet = package.Workbook.Worksheets[sheetName];
				return worksheet.Cells[row, column].Text;
			}
		}

		public string ReadCellData(string sheetName, int rowNumber, string columnName)
		{
			using (var package = new ExcelPackage(new FileInfo(filePath)))
			{
				var worksheet = package.Workbook.Worksheets[sheetName];
				if (worksheet == null || worksheet.Dimension == null)
				{
					return null; // No such worksheet or no data
				}

				// Find the column index corresponding to the given column name
				int columnIndex = -1;
				for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
				{
					if (worksheet.Cells[1, col].Text == columnName)
					{
						columnIndex = col;
						break;
					}
				}

				if (columnIndex == -1)
				{
					return null; // Column name not found
				}

				// Return the cell data using row number and column index
				return worksheet.Cells[rowNumber, columnIndex].Text;
			}
		}

		#endregion

		#region Write Cell Data
		public void WriteCellData(string sheetName, int row, int column, string value)
		{
			using (var package = new ExcelPackage(new FileInfo(filePath)))
			{
				var worksheet = package.Workbook.Worksheets[sheetName];
				worksheet.Cells[row, column].Value = value;
				package.Save();
			}
		}

		public void WriteCellData(string sheetName, int rowNumber, string columnName, string value)
		{
			using (var package = new ExcelPackage(new FileInfo(filePath)))
			{
				var worksheet = package.Workbook.Worksheets[sheetName];
				if (worksheet == null || worksheet.Dimension == null)
				{
					throw new InvalidOperationException("Worksheet does not exist or has no data.");
				}

				// Find the column index corresponding to the given column name
				int columnIndex = -1;
				for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
				{
					if (worksheet.Cells[1, col].Text == columnName)
					{
						columnIndex = col;
						break;
					}
				}

				if (columnIndex == -1)
				{
					throw new ArgumentException("Column name not found in the worksheet.");
				}

				// Write the value to the cell using row number and column index
				worksheet.Cells[rowNumber, columnIndex].Value = value;
				package.Save();
			}
		}

		#endregion

		#region Create Sheet

		/// <summary>
		/// List *LIST TYPE* columns = new List *LIST TYPE* { "Name", "Age", "Email" };
		/// </summary>

		public void CreateSheet(string sheetName, List<string> columnNames)
		{
			using (var package = new ExcelPackage(new FileInfo(filePath)))
			{
				// Check if the sheet already exists
				if (package.Workbook.Worksheets.Any(sheet => sheet.Name == sheetName))
				{
					throw new InvalidOperationException($"Sheet '{sheetName}' already exists.");
				}

				// Add a new worksheet
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

				// Add column headers
				for (int i = 0; i < columnNames.Count; i++)
				{
					worksheet.Cells[1, i + 1].Value = columnNames[i];
				}

				// Save the changes to the Excel file
				package.Save();
			}
		}

		#endregion

		#region Delete Row Data /*TODO*/

		#endregion
	}
}
