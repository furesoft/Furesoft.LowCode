namespace Furesoft.LowCode.Analyzing;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class GraphAnalyzerAttribute : Attribute
{
    public GraphAnalyzerAttribute(Type type)
    {
        Type = type;
    }

    public Type Type { get; set; }
}
