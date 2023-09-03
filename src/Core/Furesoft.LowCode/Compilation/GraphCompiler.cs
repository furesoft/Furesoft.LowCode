namespace Furesoft.LowCode.Compilation;

public class GraphCompiler
{
    public string Compile(EmptyNode entry)
    {
        var sb = new CodeWriter();

        if (entry is ICompilationNode cn)
        {
            cn.Compile(sb);
        }

        return sb.ToString();
    }
}
