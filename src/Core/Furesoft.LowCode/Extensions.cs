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

    //ToDo: maybe allow concat objects
    public static void ImportAsObject<T>(this Context context, string name = null)
    {
        var type = typeof(T);

        if (string.IsNullOrEmpty(name))
        {
            name = type.Name;
        }

        context.DefineConstructor(type, name);
    }

    public static void ImportAsSubObject<T>(this Context context, string rootObjName, string name = null)
    {
        var c = new Context();
        c.ImportAsObject<T>(name);

        context.GetVariable(rootObjName)
            .DefineProperty(name)
            .Assign(c.GetVariable(name));
    }
}
