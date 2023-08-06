namespace Furesoft.LowCode.Nodes.IO.Console;

[Description("Read a value from the console")]
[NodeCategory("IO/Console")]
[NodeView(typeof(IconNodeView),
    "M67.5375 87.1125 5.6625 148.9875A12.5 12.5 0 1023.3375 166.6625L94.05 95.95A12.5 12.5 0 0094.05 78.275L23.3375 7.5625A12.5 12.5 0 005.6625 25.2375L67.5375 87.1125zM87 173 190 172A12.5 12.5 0 00185 143L93 144A12.5 12.5 0 0086 173z")]
public class ConsoleInNode : InputOutputNode
{
    public ConsoleInNode() : base("Console Input")
    {
    }

    [Description("The input from the console")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Output { get; set; }

    public override Task Execute(CancellationToken cancellationToken)
    {
        var input = System.Console.ReadLine();
        SetOutVariable(Output, input);

        return ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
