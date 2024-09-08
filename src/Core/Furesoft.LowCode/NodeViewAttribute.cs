namespace Furesoft.LowCode;

[AttributeUsage(AttributeTargets.Class)]
public class NodeViewAttribute(Type type) : Attribute
{
    public NodeViewAttribute(Type type, object parameter) : this(type)
    {
        Parameter = parameter;
    }

    public Type Type { get; set; } = type;
    public object Parameter { get; set; }

    public int MinWidth { get; set; }
    public int MinHeight { get; set; }
}
