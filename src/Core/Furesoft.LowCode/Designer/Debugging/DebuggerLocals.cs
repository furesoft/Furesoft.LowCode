using System.ComponentModel;

namespace Furesoft.LowCode.Designer.Debugging;

public class DebuggerLocals(Dictionary<string, object> properties, Type nodeType) : CustomTypeDescriptor
{
    public override PropertyDescriptorCollection GetProperties()
    {
        var result = new PropertyDescriptorCollection(null);

        foreach (var property in properties)
        {
            result.Add(new DebuggerLocal(property, nodeType));
        }

        return result;
    }
}
