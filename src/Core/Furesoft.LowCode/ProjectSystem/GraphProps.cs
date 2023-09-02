using Furesoft.LowCode.Designer.Layout.Models.Tools.Parameters;

namespace Furesoft.LowCode.ProjectSystem;

public class GraphProps
{
    public List<PropertyDescriptor> Properties { get; set; } = new();
    public List<PinDescriptor> Pins { get; set; } = new();
}
