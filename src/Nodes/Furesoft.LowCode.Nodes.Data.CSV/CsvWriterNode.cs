using System.Data;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;
using Sylvan.Data.Csv;

namespace Furesoft.LowCode.Nodes.Data.CSV;

public class CsvWriterNode : CsvNode
{
    public CsvWriterNode() : base(TableAction.Write, "Write CSV")
    {
    }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var options = new CsvDataWriterOptions {Delimiter = GetDelimiter()};

        await using var writer = CsvDataWriter.Create(Path, options);
        var dataTable = GetTable();

        var dataTableReader = new DataTableReader(dataTable);
        await writer.WriteAsync(dataTableReader, cancellationToken);
    }
}
