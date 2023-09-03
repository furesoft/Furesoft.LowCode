using System.Text;

namespace Furesoft.LowCode.Compilation;

public class GraphCompiler
{
    public string Compile(EmptyNode entry)
    {
        var sb = new StringBuilder();

        if (entry is ICompilationNode cn)
        {
            cn.Compile(sb);
        }

        return sb.ToString();
    }
}
