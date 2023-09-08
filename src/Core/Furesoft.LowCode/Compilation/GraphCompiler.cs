namespace Furesoft.LowCode.Compilation;

public class GraphCompiler
{
    public string Compile(EmptyNode entry)
    {
        var sb = new CodeWriter();

        entry.Compile(sb);

        return sb.ToString();
    }
}
