using System.ComponentModel;

namespace Furesoft.LowCode;

public static class Extensions
{
    public static T GetAttribute<T>(this EmptyNode node)
        where T : Attribute
    {
        return TypeDescriptor.GetAttributes(node)
            .OfType<T>().FirstOrDefault();
    }
}
