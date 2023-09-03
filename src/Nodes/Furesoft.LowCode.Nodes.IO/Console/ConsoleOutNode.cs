using System.ComponentModel.DataAnnotations;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Console;

[Description("Write a text to the console")]
[NodeCategory("IO/Console")]
[NodeIcon(
    "M0 0 42 0 42 36 0 36 0 0ZM3 6 3 33 39 33 39 6 3 6ZM6.75 11 11.5 11 16.25 17.5 11.5 24 6.75 24 11.5 17.5 6.75 11Z")]
public class ConsoleOutNode : InputOutputNode, ICompilationNode
{
    public ConsoleOutNode() : base("Console Out")
    {
    }

    [Description("The text to display")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> Message { get; set; }

    public void Compile(CodeWriter builder)
    {
        builder.AppendCall("System.Console.WriteLine", Message.Source);
        builder.AppendSymbol(';');
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        System.Console.WriteLine(Message);

        return ContinueWith(OutputPin, cancellationToken);
    }
}
