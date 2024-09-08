using System.Reflection;
using NiL.JS.Core;

namespace Furesoft.LowCode.Evaluation;

public class ScriptInitalizerContext
{
    private List<IScriptModuleInitalizer> _initializers = new();

    public ScriptInitalizerContext()
    {
    }

    public void AddInitializers(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (!type.IsAssignableTo(typeof(IScriptModuleInitalizer)))
            {
                continue;
            }

            AddInitializer(type);
        }
    }

    public void AddInitializers()
    {
        var types =
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from type in assembly.GetTypes()
            where !type.IsInterface
            where type.IsAssignableTo(typeof(IScriptModuleInitalizer))
            select type;

        foreach (var type in types)
        {
            AddInitializer(type);
        }
    }

    public void AddInitializer(Type type)
    {
        var initializer = (IScriptModuleInitalizer)Activator.CreateInstance(type);
        _initializers.Add(initializer);
    }

    public void RunInitalizers(Context context)
    {
        foreach (var initializer in _initializers)
        {
            initializer.InitEngine(context);
        }
    }
}
