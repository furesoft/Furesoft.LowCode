using System.ComponentModel;
using System.Data;
using System.Text;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Nodes.Data.DataTable;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;
using SocialExplorer.IO.FastDBF;

namespace Furesoft.LowCode.Nodes.Data.Dbf;

[NodeCategory("Data/Dbf")]
[Description("Reads Dbf File to DataTable")]
[NodeIcon(
    "m0 3v14c0 1.66 4 3 9 3 5 0 9-1.34 9-3V3m0 7c0 1.66-4 3-9 3C4 13 0 11.66 0 10M18 3A9 3 0 019 6 9 3 0 010 3 9 3 0 019 0 9 3 0 0118 3Z")]
public class DbfWriterNode : DataTableNode
{
    public DbfWriterNode() : base(TableAction.Write, "Write Dbf")
    {
    }

    protected override Task Invoke(CancellationToken cancellationToken)
    {
        var table = GetTable();

        var dbfFile = new DbfFile(Encoding.GetEncoding(1252));
        dbfFile.Open(Path, FileMode.Create);

        foreach (DataColumn column in table.Columns)
        {
            dbfFile.Header.AddColumn(new(column.ColumnName, GetDbfType(column.DataType)));
        }

        WriteRecords(table, dbfFile);

        dbfFile.Close();

        return Task.CompletedTask;
    }

    private static void WriteRecords(System.Data.DataTable table, DbfFile dbfFile)
    {
        foreach (DataRow row in table.Rows)
        {
            var dbfRecord = new DbfRecord(dbfFile.Header);

            for (var colIndex = 0; colIndex < row.ItemArray.Length; colIndex++)
            {
                dbfRecord[colIndex] = row.ItemArray[colIndex].ToString();
            }

            dbfFile.Write(dbfRecord);
        }
    }

    private DbfColumn.DbfColumnType GetDbfType(Type columnDataType)
    {
        if (columnDataType == typeof(DateTime))
        {
            return DbfColumn.DbfColumnType.Date;
        }

        if (columnDataType == typeof(int))
        {
            return DbfColumn.DbfColumnType.Integer;
        }

        if (columnDataType == typeof(string))
        {
            return DbfColumn.DbfColumnType.Character;
        }

        if (columnDataType == typeof(bool))
        {
            return DbfColumn.DbfColumnType.Boolean;
        }

        if (columnDataType == typeof(float))
        {
            return DbfColumn.DbfColumnType.Float;
        }

        if (columnDataType == typeof(byte[]))
        {
            return DbfColumn.DbfColumnType.Binary;
        }

        if (columnDataType == typeof(decimal))
        {
            return DbfColumn.DbfColumnType.Number;
        }

        throw new ArgumentException($"Unsupported data type {columnDataType}");
    }
}
