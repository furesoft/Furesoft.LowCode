using Newtonsoft.Json;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Furesoft.LowCode.Evaluation;

public class Evaluatable<T>
{
    public Evaluatable(string source)
    {
        Source = source;
    }

    [JsonIgnore] public Context Context { get; set; }

    public string Source { get; set; }

    public static implicit operator T(Evaluatable<T> e)
    {
        return e.Context.Eval(e.Source).As<T>();
    }
}
