using System.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Evaluation;
using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace Furesoft.LowCode.Nodes.Data.Office;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.ImportAsObject<ScriptInitializer>("XLS");
    }

    [JavaScriptName("write")]
    public static void WriteExcel(string path, System.Data.DataTable dataTable)
    {
        using var spreadsheetDocument = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);

        var workbookpart = spreadsheetDocument.AddWorkbookPart();
        workbookpart.Workbook = new();

        var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
        worksheetPart.Worksheet = new(new SheetData());

        var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

        var sheet = new Sheet
        {
            Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
            SheetId = 1,
            Name = dataTable.TableName ?? "Sheet1"
        };
        sheets.Append(sheet);

        var worksheet = worksheetPart.Worksheet;
        var sheetData = worksheet.GetFirstChild<SheetData>();

        var headerRow = new Row();
        foreach (DataColumn column in dataTable.Columns)
        {
            var cell = new Cell();
            cell.DataType = CellValues.String;
            cell.CellValue = new(column.ColumnName);
            headerRow.AppendChild(cell);
        }

        sheetData.AppendChild(headerRow);

        foreach (DataRow dr in dataTable.Rows)
        {
            var row = new Row();
            foreach (DataColumn column in dataTable.Columns)
            {
                var cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new(dr[column].ToString());
                row.AppendChild(cell);
            }

            sheetData.AppendChild(row);
        }

        spreadsheetDocument.Save();
    }

    [JavaScriptName("read")]
    public static void ReadExcel(Evaluatable<string> path, System.Data.DataTable dataTable)
    {
        using var spreadsheetDocument = SpreadsheetDocument.Open(path, false);

        var worksheetPart = spreadsheetDocument.WorkbookPart.GetPartsOfType<WorksheetPart>().First();
        var data = worksheetPart.Worksheet.GetFirstChild<SheetData>();
        var rows = data.Descendants<Row>();

        if (dataTable.Columns.Count == 0)
        {
            foreach (Cell cell in rows.ElementAt(0))
            {
                dataTable.Columns.Add(GetCellValue(spreadsheetDocument, cell));
            }
        }

        foreach (Row row in rows.Skip(1))
        {
            var tempRow = dataTable.NewRow();

            for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
            {
                tempRow[i] = GetCellValue(spreadsheetDocument, row.Descendants<Cell>().ElementAt(i));
            }

            dataTable.Rows.Add(tempRow);
        }
    }

    private static string GetCellValue(SpreadsheetDocument document, Cell cell)
    {
        var stringTablePart = document.WorkbookPart!.SharedStringTablePart;
        var value = cell.CellValue!.InnerXml;

        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
        {
            return stringTablePart!.SharedStringTable.ChildElements[int.Parse(value)].InnerText;
        }
        else
        {
            return value;
        }
    }
}
