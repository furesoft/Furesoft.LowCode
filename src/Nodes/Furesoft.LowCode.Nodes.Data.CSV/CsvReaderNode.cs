using Furesoft.LowCode.Nodes.Data.DataTable.Core;

namespace Furesoft.LowCode.Nodes.Data.CSV;

public class CsvReaderNode() : CsvNode(TableAction.Read, "Read CSV")
{
    protected override Task Invoke(CancellationToken cancellationToken)
    {
        SetTable(ScriptInitializer.ReadCsv(Path, Delimiter, GetTable()));

        return Task.CompletedTask;
    }
}
