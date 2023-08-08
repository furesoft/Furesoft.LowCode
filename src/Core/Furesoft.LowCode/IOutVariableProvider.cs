using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Furesoft.LowCode;

public interface IOutVariableProvider
{
    [Description("Where to store the output")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Required]
    string OutVariable { get; set; }
}
