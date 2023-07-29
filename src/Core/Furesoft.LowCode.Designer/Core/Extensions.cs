using System.ComponentModel;

namespace Furesoft.LowCode.Designer.Core;

public static class Extensions
{
    public static T GetAttribute<T>(this VisualNode node)
        where T : Attribute
    {
        return TypeDescriptor.GetAttributes(node)
            .OfType<T>().FirstOrDefault();
    }
}
