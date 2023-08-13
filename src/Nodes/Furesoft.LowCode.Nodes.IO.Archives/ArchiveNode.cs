using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;
using SharpCompress.Common;
using SharpCompress.Writers;

namespace Furesoft.LowCode.Nodes.IO.Archives;

[NodeCategory("IO/FileSystem")]
public class ArchiveNode : InputOutputNode
{
    public ArchiveNode() : base("Compress Directory To Archive")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    public Evaluatable<string> Path { get; set; }
    
    [DataMember(EmitDefaultValue = false)]
    public Evaluatable<string> OutputFilename { get; set; }
    
    [DataMember(EmitDefaultValue = false)]
    public ArchiveType Type { get; set; }

    [DataMember(EmitDefaultValue = false)] public string SearchPattern { get; set; } = "*";

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await using (var zip = File.OpenWrite(OutputFilename))
        using (var zipWriter = WriterFactory.Open(zip, Type, CompressionType.Deflate))
        {
            zipWriter.WriteAll(Path, SearchPattern, SearchOption);
        }

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }

    [DataMember(EmitDefaultValue = false)]
    public SearchOption SearchOption { get; set; }
}
