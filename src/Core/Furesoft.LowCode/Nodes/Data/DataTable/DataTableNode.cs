using System.ComponentModel;
using System.Data;
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
            if (Action != TableAction.NoAction)
            {
                Description = $"{Action} '{TableName}'";
            }
        }
    }
    //@Firstname - @Lastname -> Fullname

    public BindingList<string> Transformers { get; set; } = new();


    [DataMember(EmitDefaultValue = false)] public Evaluatable<string> Path { get; set; }

    protected void ApplyTransformers()
    {
        var table = GetTable();

        foreach (var transformer in Transformers)
        {
            var parsedTransformer = TransformerRule.Parse(transformer);

            if (parsedTransformer is NewColumnRule newColumnTransformer)
            {
                var column = new DataColumn(newColumnTransformer.ColumnName);

                table.Columns.Add(column);

                foreach (DataRow row in table.Rows)
                {
                    var value = ApplyPattern(newColumnTransformer.Pattern, row);
                    row.SetField(column, value);
                }
            }
        }
    }

    private string ApplyPattern(string pattern, DataRow row)
    {
        var result = pattern;

        for (var index = 0; index < row.Table.Columns.Count; index++)
        {
            var column = row.Table.Columns[index];

            result = result.Replace("@" + column.ColumnName, row.ItemArray[index].ToString());
        }

        return result;
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await Invoke(cancellationToken);

        ApplyTransformers();

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
    private class NewColumnRule : TransformerRule
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
