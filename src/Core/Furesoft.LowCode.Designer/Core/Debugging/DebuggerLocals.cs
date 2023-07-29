using System.ComponentModel;

namespace Furesoft.LowCode.Designer.Core.Debugging;

public class DebuggerLocals : CustomTypeDescriptor
{
    private readonly Dictionary<string, object> _properties;
    private readonly Type _nodeType;

    public DebuggerLocals(Dictionary<string,object> properties, Type nodeType)
    {
        _properties = properties;
        _nodeType = nodeType;
    }

    public override PropertyDescriptorCollection GetProperties()
    {
        var result = new PropertyDescriptorCollection(null);

        foreach (var property in _properties)
        {
            result.Add(new DebuggerLocal(property, _nodeType));
        }
        
        return result;
    }
}
