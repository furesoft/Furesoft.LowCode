﻿using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;

namespace Furesoft.LowCode.Nodes.Data.CSV;

public class CsvReaderNode : CsvNode
{
    public CsvReaderNode() : base(TableAction.Read, "Read CSV")
    {
    }

    protected override Task Invoke(CancellationToken cancellationToken)
    {
        SetTable(ScriptInitializer.ReadCsv(Path, Delimiter, GetTable()));

        return Task.CompletedTask;
    }

    public override void Compile(CodeWriter builder)
    {
        CompileReadCall(builder, TableName, "CSV.read", Path, Delimiter);
    }
}
