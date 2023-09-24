using System.Data;
using Furesoft.LowCode.Compilation;
using NiL.JS.Core;
using Sylvan.Data.Csv;

namespace Furesoft.LowCode.Nodes.Data.CSV;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.DefineConstructor(typeof(CsvDelimiter));
        context.DefineVariable("writeCsv").Assign(context.GlobalContext.ProxyValue(WriteCsv));
        context.DefineVariable("readCsv").Assign(context.GlobalContext.ProxyValue(ReadCsv));
    }

    public static async void WriteCsv(string path, CsvDelimiter delimiter, System.Data.DataTable dataTable)
    {
        var options = new CsvDataWriterOptions {Delimiter = (char)delimiter};

        await using var writer = CsvDataWriter.Create(path, options);

        var dataTableReader = new DataTableReader(dataTable);

        await writer.WriteAsync(dataTableReader);
    }

    public static System.Data.DataTable ReadCsv(string path, CsvDelimiter delimiter, System.Data.DataTable dataTable)
    {
        var options = new CsvDataReaderOptions {Delimiter = (char)delimiter};
        var reader = CsvDataReader.Create(path, options);
        var indices = new Dictionary<DataColumn, int>();

        if (dataTable.Columns.Count == 0)
        {
            foreach (var column in reader.GetColumnSchema())
            {
                dataTable.Columns.Add(new DataColumn(column.ColumnName, column.DataType));
            }
        }

        foreach (DataColumn column in dataTable.Columns)
        {
            var index = reader.GetOrdinal(column.ColumnName);

            indices[column] = index;
        }

        while (reader.Read())
        {
            var row = dataTable.NewRow();

            foreach (var index in indices)
            {
                row[index.Key] = reader.GetValue(index.Value);
            }

            dataTable.Rows.Add(row);
        }

        return dataTable;
    }
}
