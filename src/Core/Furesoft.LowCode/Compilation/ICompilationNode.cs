namespace Furesoft.LowCode.Compilation;

public interface ICompilationNode
{
    void Compile(CodeWriter builder);
}
