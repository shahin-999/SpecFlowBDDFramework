using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SpecFlowBDDFramework.Utility.DataProvider
{
	public class ExcelReader
	{
		private string _filePath;

		public ExcelReader(string filePath)
		{
			_filePath = filePath;
		}

		public int GetRowCount(string sheetName)
		{
			int count = 0;

			using (SpreadsheetDocument document = SpreadsheetDocument.Open(_filePath, false))
			{
				WorkbookPart workbookPart = document.WorkbookPart;
				Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
				if (sheet != null)
				{
					WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
					count = worksheetPart.Worksheet.Descendants<Row>().Count();
				}
			}

			return count;
		}

		public List<string> ReadRow(string sheetName, int rowIndex)
		{
			List<string> rowData = new List<string>();

			using (SpreadsheetDocument document = SpreadsheetDocument.Open(_filePath, false))
			{
				WorkbookPart workbookPart = document.WorkbookPart;
				Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
				if (sheet != null)
				{
					WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
					Row row = worksheetPart.Worksheet.Descendants<Row>().ElementAtOrDefault(rowIndex);
					if (row != null)
					{
						foreach (Cell cell in row.Elements<Cell>())
						{
							rowData.Add(GetCellValue(cell, workbookPart));
						}
					}
				}
			}

			return rowData;
		}

		public string ReadCellData(string sheetName, int columnIndex, int rowIndex)
		{
			string cellData = null;

			using (SpreadsheetDocument document = SpreadsheetDocument.Open(_filePath, false))
			{
				WorkbookPart workbookPart = document.WorkbookPart;
				Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
				if (sheet != null)
				{
					WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
					Row row = worksheetPart.Worksheet.Descendants<Row>().ElementAtOrDefault(rowIndex);
					if (row != null)
					{
						Cell cell = row.Elements<Cell>().ElementAtOrDefault(columnIndex);
						if (cell != null)
						{
							cellData = GetCellValue(cell, workbookPart);
						}
					}
				}
			}

			return cellData;
		}

		public void WriteData(string sheetName, int columnIndex, int rowIndex, string data)
		{
			using (SpreadsheetDocument document = SpreadsheetDocument.Open(_filePath, true))
			{
				WorkbookPart workbookPart = document.WorkbookPart;
				Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
				if (sheet != null)
				{
					WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
					Row row = worksheetPart.Worksheet.Descendants<Row>().ElementAtOrDefault(rowIndex);
					Cell cell = row.Elements<Cell>().ElementAtOrDefault(columnIndex);
					if (cell == null)
					{
						cell = InsertCellInWorksheet(worksheetPart, columnIndex, row);
					}
					cell.CellValue = new CellValue(data);
					cell.DataType = new EnumValue<CellValues>(CellValues.String);
					worksheetPart.Worksheet.Save();
				}
			}
		}

		private Cell InsertCellInWorksheet(WorksheetPart worksheetPart, int columnIndex, Row row)
		{
			Cell refCell = null;
			foreach (Cell cell in row.Elements<Cell>())
			{
				if (string.Compare(GetColumnName(cell.CellReference.Value), GetColumnName(columnIndex)) > 0)
				{
					refCell = cell;
					break;
				}
			}

			Cell newCell = new Cell() { CellReference = GetColumnName(columnIndex) + row.RowIndex };
			worksheetPart.Worksheet.Descendants<Row>().Where(r => r.RowIndex == row.RowIndex).FirstOrDefault().InsertBefore(newCell, refCell);
			worksheetPart.Worksheet.Save();

			return newCell;
		}

		private string? GetColumnName(string? value)
		{
			throw new NotImplementedException();
		}

		private string GetCellValue(Cell cell, WorkbookPart workbookPart)
		{
			string value = cell.InnerText;
			if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
			{
				SharedStringTablePart stringTablePart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
				if (stringTablePart != null)
				{
					value = stringTablePart.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
				}
			}
			return value;
		}

		private string GetColumnName(int columnIndex)
		{
			int dividend = columnIndex;
			string columnName = String.Empty;
			int modifier;

			while (dividend > 0)
			{
				modifier = (dividend - 1) % 26;
				columnName = Convert.ToChar(65 + modifier).ToString() + columnName;
				dividend = (int)((dividend - modifier) / 26);
			}

			return columnName;
		}

	}
}
