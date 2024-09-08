namespace Furesoft.LowCode.Analyzing;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class GraphAnalyzerAttribute(Type type) : Attribute
{
    public Type Type { get; set; } = type;
}
