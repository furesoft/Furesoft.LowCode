using System.ComponentModel;

namespace Furesoft.LowCode.Designer.Core;

public static class Extensions
{
    public static T GetAttribute<T>(this EmptyNode node)
        where T : Attribute
    {
        return TypeDescriptor.GetAttributes(node)
            .OfType<T>().FirstOrDefault();
    }
}
