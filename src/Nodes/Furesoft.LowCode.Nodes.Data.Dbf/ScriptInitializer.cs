using System.Data;
using System.Text;
using Furesoft.LowCode.Compilation;
using NiL.JS.Core;
using NiL.JS.Core.Interop;
using SocialExplorer.IO.FastDBF;

namespace Furesoft.LowCode.Nodes.Data.Dbf;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.ImportAsObject<ScriptInitializer>("DBF");
    }

    [JavaScriptName("read")]
    public static void ReadDbf(string path, System.Data.DataTable dataTable)
    {
        var dbfFile = new DbfFile(Encoding.GetEncoding(1252));

        dbfFile.Open(path, FileMode.Open);

        ReadColumns(dbfFile, dataTable);
        ReadRecords(dbfFile, dataTable);

        dbfFile.Close();
    }

    [JavaScriptName("write")]
    public static void WriteDbf(string path, System.Data.DataTable table)
    {
        var dbfFile = new DbfFile(Encoding.GetEncoding(1252));
        dbfFile.Open(path, FileMode.Create);

        foreach (DataColumn column in table.Columns)
        {
            dbfFile.Header.AddColumn(new(column.ColumnName, GetDbfType(column.DataType)));
        }

        WriteRecords(table, dbfFile);

        dbfFile.Close();
    }

    private static DbfColumn.DbfColumnType GetDbfType(Type columnDataType)
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

    private static void ReadRecords(DbfFile dbfFile, System.Data.DataTable dataTable)
    {
        for (var recordIndex = 0; recordIndex < dbfFile.Header.RecordCount; recordIndex++)
        {
            var record = dbfFile.Read(recordIndex);
            var row = dataTable.NewRow();

            for (var colIndex = 0; colIndex < record.ColumnCount; colIndex++)
            {
                //ToDo: convert resulting string to row datatype
                row[colIndex] = record[colIndex];
            }
        }
    }

    private static void ReadColumns(DbfFile dbfFile, System.Data.DataTable dataTable)
    {
        for (var colIndex = 0; colIndex < dbfFile.Header.ColumnCount; colIndex++)
        {
            var dbfColumn = dbfFile.Header[colIndex];
            var column = new DataColumn(dbfColumn.Name, GetTypeFromDbfType(dbfColumn.ColumnType));

            dataTable.Columns.Add(column);
        }
    }

    private static Type GetTypeFromDbfType(DbfColumn.DbfColumnType dbfColumnType)
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
