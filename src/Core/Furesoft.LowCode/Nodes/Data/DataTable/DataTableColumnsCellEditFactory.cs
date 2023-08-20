﻿using Avalonia.PropertyGrid.Controls;
using Avalonia.PropertyGrid.Controls.Factories;

namespace Furesoft.LowCode.Nodes.Data.DataTable;

public class DataTableColumnsCellEditFactory : AbstractCellEditFactory
{
    public override Control HandleNewProperty(PropertyCellContext context)
    {
        if (context.Property.PropertyType != typeof(ObservableCollection<ColumnDefinition>))
        {
            return null;
        }


        var btn = new Button();
        btn.Content = "...";
        btn.Tag = context.Property.GetValue(context.Target);
        btn.Click += (sender, _) =>
        {
            var wndw = new ColumnsWindow();
            wndw.DataContext = context.Target;

            wndw.Show();
        };

        return btn;
    }

    public override bool HandlePropertyChanged(PropertyCellContext context)
    {
        if (context.Property.PropertyType != typeof(ObservableCollection<ColumnDefinition>))
        {
            return false;
        }

        ValidateProperty(context.CellEdit, context.Property, context.Target);

        if (context.CellEdit is Button btn)
        {
            return true;
        }

        return true;
    }
}
