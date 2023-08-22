using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Furesoft.LowCode.Nodes.Data.DataTable;

public abstract class DataTableNode : InputOutputNode
{
    public TableAction Action { get; }
    private string _tableName;

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

    [DataMember(EmitDefaultValue = false)]
    public Evaluatable<string> Path { get; set; }
    public DataTableNode(TableAction action, string label) : base(label)
    {
        Action = action;
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await Invoke(cancellationToken);

        await ContinueWith(OutputPin, cancellationToken);
    }

    protected System.Data.DataTable GetTable()
    {
        var value = Context.GetVariable(TableName);

        return value != JSValue.NotExists ? value.As<System.Data.DataTable>() : null;
    }

    protected void SetTable(System.Data.DataTable table)
    {
        DefineConstant(TableName, table);
    }

    protected abstract Task Invoke(CancellationToken cancellationToken);
}
