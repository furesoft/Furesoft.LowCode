﻿using System.Data;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Nodes.Data.DataTable;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;
using Furesoft.LowCode.NodeViews;
using Sylvan.Data.Csv;

namespace Furesoft.LowCode.Nodes.Data.CSV;

[NodeCategory("Data/CSV")]
[NodeView(typeof(IconNodeView),
    "M224 136V0H24C10.7 0 0 10.7 0 24v464c0 13.3 10.7 24 24 24h336c13.3 0 24-10.7 24-24V160H248c-13.2 0-24-10.8-24-24zm-96 144c0 4.42-3.58 8-8 8h-8c-8.84 0-16 7.16-16 16v32c0 8.84 7.16 16 16 16h8c4.42 0 8 3.58 8 8v16c0 4.42-3.58 8-8 8h-8c-26.51 0-48-21.49-48-48v-32c0-26.51 21.49-48 48-48h8c4.42 0 8 3.58 8 8v16zm44.27 104H160c-4.42 0-8-3.58-8-8v-16c0-4.42 3.58-8 8-8h12.27c5.95 0 10.41-3.5 10.41-6.62 0-1.3-.75-2.66-2.12-3.84l-21.89-18.77c-8.47-7.22-13.33-17.48-13.33-28.14 0-21.3 19.02-38.62 42.41-38.62H200c4.42 0 8 3.58 8 8v16c0 4.42-3.58 8-8 8h-12.27c-5.95 0-10.41 3.5-10.41 6.62 0 1.3.75 2.66 2.12 3.84l21.89 18.77c8.47 7.22 13.33 17.48 13.33 28.14.01 21.29-19 38.62-42.39 38.62zM256 264v20.8c0 20.27 5.7 40.17 16 56.88 10.3-16.7 16-36.61 16-56.88V264c0-4.42 3.58-8 8-8h16c4.42 0 8 3.58 8 8v20.8c0 35.48-12.88 68.89-36.28 94.09-3.02 3.25-7.27 5.11-11.72 5.11s-8.7-1.86-11.72-5.11c-23.4-25.2-36.28-58.61-36.28-94.09V264c0-4.42 3.58-8 8-8h16c4.42 0 8 3.58 8 8zm121-159L279.1 7c-4.5-4.5-10.6-7-17-7H256v128h128v-6.1c0-6.3-2.5-12.4-7-16.9z")]
public class CsvWriterNode : DataTableNode
{
    public CsvWriterNode() : base(TableAction.Write, "Write CSV")
    {
    }

    [DataMember(EmitDefaultValue = false)] public CsvDelimiter Delimiter { get; set; } = CsvDelimiter.Semikolon;

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var options = new CsvDataWriterOptions {Delimiter = GetDelimeter()};

        await using var writer = CsvDataWriter.Create(Path, options);
        var dataTable = GetTable();

        var dataTableReader = new DataTableReader(dataTable);
        await writer.WriteAsync(dataTableReader, cancellationToken);
    }

    private char GetDelimeter()
    {
        return Delimiter switch
        {
            CsvDelimiter.Comma => ',',
            CsvDelimiter.Semikolon => ';',
            _ => ';'
        };
    }
}
