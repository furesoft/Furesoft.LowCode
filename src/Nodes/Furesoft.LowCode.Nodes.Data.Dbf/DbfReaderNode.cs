using System.ComponentModel;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Nodes.Data.DataTable;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;

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
        ScriptInitializer.ReadDbf(Path, GetTable());

        return Task.CompletedTask;
    }

    public override void Compile(CodeWriter builder)
    {
        CompileReadCall(builder, "DBF.read", Path);
    }
}
