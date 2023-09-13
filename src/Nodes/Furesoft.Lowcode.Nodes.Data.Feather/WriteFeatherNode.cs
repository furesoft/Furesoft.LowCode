using System.ComponentModel;
using FeatherDotNet;
using Furesoft.LowCode;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Nodes.Data.DataTable;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;

namespace Furesoft.Lowcode.Nodes.Data.Feather;

[NodeCategory("Data/Feather")]
[NodeIcon(
    "M1.254 19.567C1.561 18.585 2.024 17.203 2.645 15.205 5.352 14.776 6.472 15.546 8.191 12.476 6.796 12.903 5.114 11.684 5.204 11.155 5.295 10.627 9.117 11.536 11.62 7.982 8.465 8.678 7.456 7.146 7.863 6.915 8.802 6.381 11.589 6.693 13.075 5.246 13.841 4.501 14.2 2.69 13.888 2.044 13.514 1.263 11.232.098 9.974.208 8.716.317 6.743 4.998 6.157 4.962 5.573 4.925 5.454 2.864 6.476.949 5.399 1.426 3.425 2.908 2.806 4.175 1.653 6.532 2.914 11.941 2.51 12.133 2.105 12.326.744 9.652.338 8.439-.217 10.298-.23 12.16 1.391 14.633.78 16.256.446 18.124.395 19.074.371 19.833 1.119 19.996 1.254 19.567Z")]
[Description("Write DataTable To Feather File")]
public class WriteFeatherNode : DataTableNode
{
    public WriteFeatherNode() : base(TableAction.Write, "Write Feather")
    {
    }

    protected override Task Invoke(CancellationToken cancellationToken)
    {
        var table = GetTable();
        using var writer = new FeatherWriter(Path);

        for (var columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
        {
            var column = table.Columns[columnIndex];
            var columnArray = new List<object>();

            for (var rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
            {
                columnArray.Add(table.Rows[rowIndex].ItemArray[columnIndex]);
            }

            writer.AddColumn(column.ColumnName, columnArray);
        }

        return Task.CompletedTask;
    }
}
