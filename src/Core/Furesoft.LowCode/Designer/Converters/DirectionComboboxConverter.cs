namespace Furesoft.LowCode.Designer.Converters;

public class DirectionComboboxConverter : ValueConverter<DirectionComboboxConverter, ComboBoxItem>
{
    protected override object Convert(ComboBoxItem value, Type targetType, object parameter)
    {
        return value.Content;
    }

    public override object ConvertBack(ComboBoxItem value, Type targetType, object parameter)
    {
        return Enum.Parse<PinMode>(value.Content.ToString());
    }
}
