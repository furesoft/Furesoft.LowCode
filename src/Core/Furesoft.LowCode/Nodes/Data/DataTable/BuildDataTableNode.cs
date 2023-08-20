using System.ComponentModel;
using System.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode.Nodes.Data.DataTable;

[NodeCategory("Data")]
[Description("Build a structure to save data in a table")]
public class BuildDataTableNode : InputOutputNode, IOutVariableProvider
{
    public BuildDataTableNode() : base("Build DataTable")
    {
        AddColumnCommand = new RelayCommand(AddColumn);
        AddRowCommand = new RelayCommand(AddRow);
    }

    private void AddRow()
    {
        Rows.Add(new RowDataDefinition());
    }

    [Browsable(false)] public System.Data.DataTable Table { get; set; }

    public ObservableCollection<ColumnDataDefinition> Columns { get; set; } = new();
    public ObservableCollection<RowDataDefinition> Rows { get; set; } = new();

    [Browsable(false)] public ICommand AddColumnCommand { get; set; }
    [Browsable(false)] public ICommand AddRowCommand { get; set; }

    public string OutVariable { get; set; }

    private void AddColumn()
    {
        Columns.Add(new());
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        Table = new();
        Table.TableName = OutVariable;
        Table.Columns.AddRange(Columns
            .Select(_ => new DataColumn {ColumnName = _.ColumnName, ReadOnly = _.IsReadOnly, DataType = _.DataType})
            .ToArray());

        DefineConstant(OutVariable, Table);

        await ContinueWith(OutputPin, cancellationToken);
    }
}
