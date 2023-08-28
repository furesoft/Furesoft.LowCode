using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;

namespace Furesoft.LowCode.Nodes.Data.DataTable;

[NodeCategory("Data")]
public class RemoveColumnNode : DataTableNode
{
    [DataMember(EmitDefaultValue = false)]
    public string ColumnName { get; set; }
    public RemoveColumnNode() : base(TableAction.Remove, "Column")
    {
    }

    protected override Task Invoke(CancellationToken cancellationToken)
    {
        var table = GetTable();

        table.Columns.Remove(ColumnName);

        return Task.CompletedTask;
    }
}
