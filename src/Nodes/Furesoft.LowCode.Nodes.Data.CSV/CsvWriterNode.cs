using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;

namespace Furesoft.LowCode.Nodes.Data.CSV;

public class CsvWriterNode : CsvNode
{
    public CsvWriterNode() : base(TableAction.Write, "Write CSV")
    {
    }

    protected override Task Invoke(CancellationToken cancellationToken)
    {
        ScriptInitializer.WriteCsv(Path, Delimiter, GetTable());

        return Task.CompletedTask;
    }

    public override void Compile(CodeWriter builder)
    {
        CompileWriteCall(builder, "CSV.write", Path, Delimiter, TableName.AsEvaluatable());
    }
}
