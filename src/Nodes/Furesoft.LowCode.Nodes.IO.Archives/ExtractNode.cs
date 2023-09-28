using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Archives;

[NodeCategory("IO/FileSystem")]
[NodeIcon("M1 18H17V5H1M11 12V15H7V12H4L9 7 14 12M0 0H18V4H0")]
public class ExtractNode : InputOutputNode
{
    public ExtractNode() : base("Extract Archive")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> ArchiveFilename { get; set; }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> OutputDirectory { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        ScriptInitializer.Extract(ArchiveFilename, OutputDirectory);

        await ContinueWith(OutputPin, cancellationToken);
    }

    public override void Compile(CodeWriter builder)
    {
        CompileWriteCall(builder, "Archive.extract", ArchiveFilename, OutputDirectory);
    }
}
