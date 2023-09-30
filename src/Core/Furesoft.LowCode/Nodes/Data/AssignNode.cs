using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;

namespace Furesoft.LowCode.Nodes.Data;

[NodeCategory("Data")]
[NodeIcon(
    "M931.755 599.68c0 0.043-0.043 0.043 0 0l-0.128 0.171-0.043 0.043v0.043c-2.679 3.908-6.019 7.153-9.89 9.646l-0.137 0.082-390.997 259.413c-5.42 3.51-12.045 5.597-19.157 5.597s-13.738-2.087-19.297-5.682l0.139 0.084-391.083-259.541c-9.569-6.017-15.836-16.519-15.836-28.484 0-0.231 0.002-0.461 0.007-0.691l-0.001 0.034v-262.827c-0.003-0.158-0.004-0.345-0.004-0.532 0-6.921 2.089-13.355 5.67-18.704l-0.077 0.122v-0.043l0.085-0.085 0.427-0.64v-0.085h0.043l0.085-0.085c2.688-3.755 5.973-6.827 9.643-9.173l391.040-259.499c5.325-3.712 11.931-5.931 19.056-5.931 0.021 0 0.041 0 0.062 0h-0.003c0.055 0 0.119-0.001 0.184-0.001 7.112 0 13.705 2.219 19.125 6.003l-0.109-0.072 391.040 259.499c9.574 6.095 15.834 16.653 15.834 28.672 0 0.195-0.002 0.389-0.005 0.584v-0.029 262.827c0.003 0.156 0.004 0.339 0.004 0.523 0 6.993-2.121 13.49-5.755 18.883l0.077-0.121zM545.323 777.685l296.661-196.821-135.808-90.112-160.896 108.203v178.731zM477.397 777.685v-178.731l-160.896-108.203-135.723 90.069 296.619 196.864zM153.344 517.504l102.187-67.84-102.187-68.736v136.576zM477.397 120.235l-296.96 196.992 136.32 91.733 160.64-106.581v-182.144zM511.317 361.429l-133.461 88.661 133.461 89.813 133.589-89.813-133.589-88.661zM545.323 120.235v182.187l160.683 106.581 136.235-91.733-296.917-197.035zM869.461 380.928l-102.229 68.736 102.229 67.84v-136.576z")]
[Description("Save a value for later usage")]
public class AssignNode : InputOutputNode
{
    private string _name;

    public AssignNode() : base("Assign Variable")
    {
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Description("The name of the variable")]
    [Required]
    public string Name
    {
        get => _name;
        set
        {
            SetProperty(ref _name, value);

            Description = $"Assign '{value}'";
        }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Description("The value of the variable")]
    [Required]
    public Evaluatable<object> Value { get; set; }

    public override void Compile(CodeWriter builder)
    {
        builder.AppendIdentifier(Name)
            .AppendSymbol('=')
            .Append(Value.Source, false)
            .AppendSymbol(';');
    }
}
