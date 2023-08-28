using System.ComponentModel;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;

namespace Furesoft.LowCode.Nodes.Data.DataTable;

[NodeCategory("Data")]
public class RemoveColumnsNode : DataTableNode
{
    public BindingList<string> Columns { get; set; }

    public RemoveColumnsNode() : base(TableAction.None, "Remove Columns")
    {
    }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var dataTable = GetTable();

        foreach (var column in Columns)
        {
            if (!dataTable.Columns.Contains(column))
            {
                throw CreateError($"Column '{column}' does not exists");
            }

            dataTable.Columns.Remove(column);
        }
    }
}
