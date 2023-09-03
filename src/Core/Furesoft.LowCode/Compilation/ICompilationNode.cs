using System.Text;

namespace Furesoft.LowCode.Compilation;

public interface ICompilationNode
{
    void Compile(StringBuilder builder);
}
