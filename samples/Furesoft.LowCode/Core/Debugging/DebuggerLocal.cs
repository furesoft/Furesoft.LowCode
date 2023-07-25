using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Furesoft.LowCode.Core.Debugging;

public class DebuggerLocal : PropertyDescriptor
{
    private readonly KeyValuePair<string, object> _property;

    public DebuggerLocal(KeyValuePair<string,object> property, Type nodeType) : base(property.Key, null)
    {
        _property = property;
        ComponentType = nodeType;
    }

    public override bool CanResetValue(object component)
    {
        return false;
    }

    public override object GetValue(object component)
    {
        return _property.Value;
    }

    public override void ResetValue(object component)
    {
        
    }

    public override void SetValue(object component, object value)
    {
        
    }

    public override bool ShouldSerializeValue(object component)
    {
        return false;
    }

    public override Type ComponentType { get; }
    public override bool IsReadOnly => true;
    public override Type PropertyType => _property.Value.GetType();
}
