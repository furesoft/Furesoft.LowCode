using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.NodeViews;
using SharpCompress.Readers;

namespace Furesoft.LowCode.Nodes.IO.Archives;

[NodeCategory("IO/FileSystem")]
[NodeView(typeof(IconNodeView), "M1 18H17V5H1M11 12V15H7V12H4L9 7 14 12M0 0H18V4H0")]
public class ExtractNode : InputOutputNode
{
    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> ArchiveFilename { get; set; }
    
    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> OutputDirectory { get; set; }
    
    public ExtractNode() : base("Extract Archive")
    {
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await using Stream stream = File.OpenRead(ArchiveFilename);
        var reader = ReaderFactory.Open(stream);
        
        while (reader.MoveToNextEntry())
        {
            if (reader.Entry.IsDirectory)
            {
                continue;
            }

            Console.WriteLine(reader.Entry.Key);
            reader.WriteEntryToDirectory(OutputDirectory, new() { ExtractFullPath = true, Overwrite = true });
        }

        await ContinueWith(OutputPin, cancellationToken);
    }
}
