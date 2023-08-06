namespace Furesoft.LowCode.Analyzing;

public class GraphAnalyzerAttribute : Attribute
{
    public GraphAnalyzerAttribute(Type type)
    {
        Type = type;
    }

    public Type Type { get; set; }
}
