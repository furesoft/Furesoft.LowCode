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
public class DbfReaderNode : DataTableNode
{
    public DbfReaderNode() : base(TableAction.Read, "Read Dbf")
    {
    }

    protected override Task Invoke(CancellationToken cancellationToken)
    {
        var dataTable = GetTable();
        var dbfFile = new DbfFile(Encoding.GetEncoding(1252));

        dbfFile.Open(Path, FileMode.Open);

        ReadColumns(dbfFile, dataTable);
        ReadRecords(dbfFile, dataTable);

        dbfFile.Close();

        return Task.CompletedTask;
    }

    private static void ReadRecords(DbfFile dbfFile, System.Data.DataTable dataTable)
    {
        for (int recordIndex = 0; recordIndex < dbfFile.Header.RecordCount; recordIndex++)
        {
            var record = dbfFile.Read(recordIndex);
            var row = dataTable.NewRow();

            for (int colIndex = 0; colIndex < record.ColumnCount; colIndex++)
            {
                //ToDo: convert resulting string to row datatype
                row[colIndex] = record[colIndex];
            }
        }
    }

    private void ReadColumns(DbfFile dbfFile, System.Data.DataTable dataTable)
    {
        for (int colIndex = 0; colIndex < dbfFile.Header.ColumnCount; colIndex++)
        {
            var dbfColumn = dbfFile.Header[colIndex];
            var column = new DataColumn(dbfColumn.Name, GetTypeFromDbfType(dbfColumn.ColumnType));

            dataTable.Columns.Add(column);
        }
    }

    private Type GetTypeFromDbfType(DbfColumn.DbfColumnType dbfColumnType)
    {
        return dbfColumnType switch
        {
            DbfColumn.DbfColumnType.Date => typeof(DateTime),
            DbfColumn.DbfColumnType.Integer => typeof(int),
            DbfColumn.DbfColumnType.Character => typeof(string),
            DbfColumn.DbfColumnType.Boolean => typeof(bool),
            DbfColumn.DbfColumnType.Float => typeof(float),
            DbfColumn.DbfColumnType.Binary => typeof(byte[]),
            DbfColumn.DbfColumnType.Number => typeof(decimal),

            _ => throw new ArgumentException($"Unsupported column data type {dbfColumnType}")
        };
    }
}
