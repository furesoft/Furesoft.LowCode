﻿using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Nodes.Data.DataTable;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;
using NiL.JS.Core;

namespace Furesoft.LowCode.Nodes.IO.Linq;

[NodeCategory("Linq")]
public class ObjectToDatatableNode : DataTableNode
{
    public JSValue Obj { get; set; }
    public ObjectToDatatableNode() : base(TableAction.None, "Push Object To Datatable")
    {
    }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var table = GetTable();
        ApplyPipe<object>();

        foreach (var prop in Obj)
        {
        }
            table.Rows.Add(Obj);
    }
}
