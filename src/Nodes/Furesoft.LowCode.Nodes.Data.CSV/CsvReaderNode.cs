using System.Data;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;
using Sylvan.Data.Csv;

namespace Furesoft.LowCode.Nodes.Data.CSV;

public class CsvReaderNode : CsvNode
{
    public CsvReaderNode() : base(TableAction.Read, "Read CSV")
    {
    }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var options = new CsvDataReaderOptions {Delimiter = GetDelimiter()};
        var reader = CsvDataReader.Create(Path, options);
        var dataTable = GetTable();
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

        while (await reader.ReadAsync(cancellationToken))
        {
            var row = dataTable.NewRow();

            foreach (var index in indices)
            {
                row[index.Key] = reader.GetValue(index.Value);
            }

            dataTable.Rows.Add(row);
        }
    }
}
