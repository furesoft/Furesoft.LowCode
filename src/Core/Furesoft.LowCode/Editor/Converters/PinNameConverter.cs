namespace Furesoft.LowCode.Editor.Converters;

public class PinNameConverter : ValueConverter<PinNameConverter, PinViewModel>
{
    protected override object Convert(PinViewModel vm, Type targetType, object parameter)
    {
        return vm.Mode + ": " + vm.Name;
    }
}
