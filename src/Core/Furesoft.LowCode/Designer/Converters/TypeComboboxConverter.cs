﻿namespace Furesoft.LowCode.Designer.Converters;

public class TypeComboboxConverter : ValueConverter<TypeComboboxConverter, ComboBoxItem>
{
    protected override object Convert(ComboBoxItem value, Type targetType, object parameter)
    {
        return value.Content;
    }

    public override object ConvertBack(ComboBoxItem value, Type targetType, object parameter)
    {
        return value.Content;
    }
}
