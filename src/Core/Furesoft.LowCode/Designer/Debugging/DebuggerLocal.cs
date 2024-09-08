using System.ComponentModel;

namespace Furesoft.LowCode.Designer.Debugging;

public class DebuggerLocal(KeyValuePair<string, object> property, Type nodeType)
    : PropertyDescriptor(property.Key, null)
{
    public override Type ComponentType { get; } = nodeType;
    public override bool IsReadOnly => true;
    public override Type PropertyType => property.Value.GetType();

    public override bool CanResetValue(object component)
    {
        return false;
    }

    public override object GetValue(object component)
    {
        return property.Value;
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
}
