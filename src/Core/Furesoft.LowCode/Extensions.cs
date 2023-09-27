using NiL.JS.Core;

namespace Furesoft.LowCode;

public static class StringExtensions
{
    public static Evaluatable<string> AsEvaluatable(this string s)
    {
        return new(s);
    }

    public static void Import(this Context context, string name, object value)
    {
        context.DefineConstant(name, context.GlobalContext.ProxyValue(value));
    }

    public static void Import(this Context context, Type type)
    {
        context.DefineConstructor(type);
    }

    public static void ImportAsObject<T>(this Context context, string name = null)
    {
        Type type = typeof(T);

        if (string.IsNullOrEmpty(name))
        {
            name = type.Name;
        }

        context.DefineConstructor(type, name);
    }
}
