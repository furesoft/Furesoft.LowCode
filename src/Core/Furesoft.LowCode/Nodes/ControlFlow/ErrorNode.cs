using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;

namespace Furesoft.LowCode.Nodes.ControlFlow;

[NodeCategory("Control Flow")]
[Description("Throw an error and abort the execution")]
[NodeIcon(
    "M7.25 4A3.25 3.25 0 0110.5.75 3.25 3.25 0 0113.75 4C13.75 4.42 14.08 4.75 14.5 4.75 14.92 4.75 15.25 4.42 15.25 4V3.25H16.75V4A2.25 2.25 0 0114.5 6.25 2.25 2.25 0 0112.25 4 1.75 1.75 0 0010.5 2.25 1.75 1.75 0 008.75 4H10V5.29C12.89 6.15 15 8.83 15 12A7 7 0 018 19 7 7 0 011 12C1 8.83 3.11 6.15 6 5.29V4H7.25M18 4H20V5H18V4M15 2V0H16V2H15M16.91 2.38 18.33.96 19.04 1.67 17.62 3.09 16.91 2.38Z")]
public class ErrorNode : InputNode
{
    public ErrorNode() : base("Error")
    {
    }

    [Description("The error message")]
    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> Message { get; set; }

    public override void Compile(CodeWriter builder)
    {
        builder.Throw(Message.Source);
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        throw CreateError(Message);
    }
}
