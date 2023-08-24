using Avalonia.Controls.Templates;
using Avalonia.Interactivity;

namespace Furesoft.LowCode.Nodes.Data.DataTable.Core;

public partial class RowsWindow : Window
{
    public RowsWindow()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        var k = (BuildDataTableNode)DataContext;

        foreach (var column in k.Columns)
        {
            DataGrid.Columns.Add(new DataGridTemplateColumn
            {
                Header = column.ColumnName,
                IsReadOnly = column.IsReadOnly,
                CellTemplate = new FuncDataTemplate<RowDataDefinition>((row, e) =>
                {
                    if (column.DataType == typeof(bool))
                    {
                        var cb = new CheckBox();
                        cb.IsChecked = row.GetProperty<bool>(column.ColumnName);

                        cb.IsCheckedChanged += (s, ee) =>
                        {
                            row.SetProperty(column.ColumnName, cb.IsChecked);
                        };

                        return cb;
                    }
                    else if (column.DataType == typeof(string))
                    {
                        var tb = new TextBox();
                        tb.Text = row.GetProperty<string>(column.ColumnName);

                        tb.TextChanged += (s, ee) =>
                        {
                            row.SetProperty(column.ColumnName, tb.Text);
                        };

                        return tb;
                    }

                    return null;
                })
            });
        }
    }
}
