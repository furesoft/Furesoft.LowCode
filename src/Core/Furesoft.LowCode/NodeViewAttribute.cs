namespace Furesoft.LowCode;

[AttributeUsage(AttributeTargets.Class)]
public class NodeViewAttribute : Attribute
{
    public Type Type { get; set; }
    public object Parameter { get; set; }

    public int MinWidth { get; set; }
    public int MinHeight { get; set; }
    
    public NodeViewAttribute(Type type)
    {
        Type = type;
    }

    public NodeViewAttribute(Type type, object parameter) : this(type)
    {
        Parameter = parameter;
    }

}
