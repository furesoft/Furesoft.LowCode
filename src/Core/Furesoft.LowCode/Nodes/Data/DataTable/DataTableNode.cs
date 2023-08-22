using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Furesoft.LowCode.Nodes.Data.DataTable;

public abstract class DataTableNode : InputOutputNode
{
    private string _tableName;

    protected DataTableNode(TableAction action, string label) : base(label)
    {
        Action = action;
    }

    public TableAction Action { get; }

    [DataMember(EmitDefaultValue = false)]
    public string TableName
    {
        get => _tableName;
        set
        {
            SetProperty(ref _tableName, value);
            Description = $"{Action} '{TableName}'";
        }
    }

    [DataMember(EmitDefaultValue = false)] public Evaluatable<string> Path { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await Invoke(cancellationToken);

        await ContinueWith(OutputPin, cancellationToken);
    }

    protected System.Data.DataTable GetTable()
    {
        var value = Context.GetVariable(TableName);

        if (value == JSValue.NotExists)
        {
            var dt = new System.Data.DataTable();
            SetTable(dt);

            return dt;
        }

        return value.As<System.Data.DataTable>();
    }

    protected void SetTable(System.Data.DataTable table)
    {
        DefineConstant(TableName, table);
    }

    protected abstract Task Invoke(CancellationToken cancellationToken);
}
