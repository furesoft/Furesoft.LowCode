using Newtonsoft.Json;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Furesoft.LowCode.Evaluation;

public class Evaluatable<T>(string source)
{
    [JsonIgnore] public Context Context { get; set; }

    public string Source { get; set; } = source;

    public static implicit operator T(Evaluatable<T> e)
    {
        if (e is null) return default;

        return e.Context.Eval(e.Source).As<T>();
    }
}
