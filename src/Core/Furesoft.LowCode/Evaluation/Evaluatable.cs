using NiL.JS.Extensions;

namespace Furesoft.LowCode.Evaluation;

public class Evaluatable<T>
{
    public EmptyNode Parent { get; set; }

    public Evaluatable(string source)
    {
        Source = source;
    }

    public string Source { get; set; }

    public static implicit operator T(Evaluatable<T> e)
    {
       return e.Parent.Context.Eval(e.Source).As<T>();
    }
}
