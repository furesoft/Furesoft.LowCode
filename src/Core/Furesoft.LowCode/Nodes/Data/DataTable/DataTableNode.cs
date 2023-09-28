using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;
using Furesoft.LowCode.Compilation;
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

    [Browsable(false)] [JsonIgnore] public TableAction Action { get; }

    [DataMember(EmitDefaultValue = false)]
    public string TableName
    {
        get => _tableName;
        set
        {
            SetProperty(ref _tableName, value);

            if (Action != TableAction.None)
            {
                Description = $"{Action} '{TableName}'";
            }
        }
    }
    //@Firstname - @Lastname -> Fullname

    public BindingList<string> Transformers { get; set; } = new();


    [DataMember(EmitDefaultValue = false)] public Evaluatable<string> Path { get; set; }
    public object PipeVariable { get; set; }

    private void ApplyTransformers()
    {
        var table = GetTable();

        foreach (var transformer in Transformers)
        {
            var parsedTransformer = TransformerRule.Parse(transformer);

            if (parsedTransformer is NewColumnRule newColumnTransformer)
            {
                var column = new DataColumn(newColumnTransformer.ColumnName);

                if (!table.Columns.Contains(column.ColumnName))
                {
                    continue;
                }

                CalculateColumnValues(table, column, newColumnTransformer);

                table.Columns.Add(column);
            }
        }
    }

    private void CalculateColumnValues(System.Data.DataTable table, DataColumn column,
        NewColumnRule newColumnTransformer)
    {
        foreach (DataRow row in table.Rows)
        {
            var context = new Context(Context.GlobalContext);

            for (var index = 0; index < row.Table.Columns.Count; index++)
            {
                var c = row.Table.Columns[index];

                context.DefineConstant(c.ColumnName, row.ItemArray[index].ToString());
            }

            row.SetField(column, context.Eval(newColumnTransformer.Pattern));
        }
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        ApplyPipe<System.Data.DataTable>();

        await Invoke(cancellationToken);

        ApplyTransformers();

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

    private class TransformerRule
    {
        public static TransformerRule Parse(string src)
        {
            if (src.Contains(':'))
            {
                return NewColumnRule.Parse(src);
            }

            return null;
        }
    }

    //Fullname: @Firstname - @Lastname
    private sealed class NewColumnRule : TransformerRule
    {
        public string ColumnName { get; set; }
        public string Pattern { get; set; }

        public new static NewColumnRule Parse(string src)
        {
            var result = new NewColumnRule();

            var spl = src.Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            result.ColumnName = spl[0];
            result.Pattern = spl[1];

            return result;
        }
    }
}
