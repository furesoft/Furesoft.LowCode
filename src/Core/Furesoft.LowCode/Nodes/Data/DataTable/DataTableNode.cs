using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;
using Newtonsoft.Json;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Furesoft.LowCode.Nodes.Data.DataTable;

public abstract class DataTableNode : InputOutputNode, IPipeable
{
    private string _tableName;

    protected DataTableNode(TableAction action, string label) : base(label)
    {
        Action = action;
    }

    [Browsable(false), JsonIgnore]
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
    public object PipeVariable { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        ApplyPipe<System.Data.DataTable>();

        await Invoke(cancellationToken);

        await ContinueWith(OutputPin, cancellationToken);
    }

    protected System.Data.DataTable GetTable()
    {
        if (PipeVariable is System.Data.DataTable pipedDataTable)
        {
            return pipedDataTable;
        }

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

        PipeVariable = table;
    }

    protected abstract Task Invoke(CancellationToken cancellationToken);
}
