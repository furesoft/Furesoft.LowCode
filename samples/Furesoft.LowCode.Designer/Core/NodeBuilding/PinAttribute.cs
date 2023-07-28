using System;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Designer.Core.NodeBuilding;

[AttributeUsage(AttributeTargets.Property)]
public class PinAttribute : Attribute
{
    public PinAttribute(string name = null, PinAlignment alignment = PinAlignment.None, bool allowMultipleConnections = true)
    {
        Name = name;
        Alignment = alignment;
        AllowMultipleConnections = allowMultipleConnections;
    }

    public PinAlignment Alignment { get; set; }
    public bool AllowMultipleConnections { get; }
    public string Name { get; set; }
}
