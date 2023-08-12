namespace Furesoft.LowCode.Designer.Converters;

public class ProjectNameConverter : ValueConverter<ProjectNameConverter, string>
{
    protected override object Convert(string value, Type targetType, object parameter)
    {
        return "Project " + value;
    }
}
