﻿using NiL.JS.Core;
using PropertyModels.Extensions;

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

    public static void ImportAsObject<T>(this Context context, string name = null) {
        Type type = typeof(T);
        
        if(!type.IsStatic()) {
            throw new InvalidOperationException($"Type '{type.FullName}' must be static");
        }

        if(string.IsNullOrEmpty(name)) {
            name = type.Name;
        }

        //ToDo: falls das nicht funktioniert 'DefineConstruktor" probieren
        context.DefineConstant(name, context.GlobalContext.WrapValue(Activator.CreateInstance(type)));
    }
}