namespace Furesoft.LowCode.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PinAttribute(
    string name = null,
    PinAlignment alignment = PinAlignment.Left,
    bool allowMultipleConnections = true)
    : Attribute
{
    public PinAlignment Alignment { get; set; } = alignment;
    public bool AllowMultipleConnections { get; } = allowMultipleConnections;
    public string Name { get; set; } = name;
}
