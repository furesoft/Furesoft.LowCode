using Furesoft.LowCode.Nodes.Data.DataTable.Core;

namespace Furesoft.LowCode.Nodes.Data.CSV;

public class CsvWriterNode() : CsvNode(TableAction.Write, "Write CSV")
{
    protected override Task Invoke(CancellationToken cancellationToken)
    {
        ScriptInitializer.WriteCsv(Path, Delimiter, GetTable());

        return Task.CompletedTask;
    }
}
