using System.ComponentModel;
using System.Runtime.Serialization;

namespace Furesoft.LowCode.Designer.Core;

public interface IOutVariableProvider
{
    [Description("Where to store the output")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    string OutVariable { get; set; }
}
