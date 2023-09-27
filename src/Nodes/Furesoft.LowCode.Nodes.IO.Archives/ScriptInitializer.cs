using Furesoft.LowCode.Compilation;
using NiL.JS.Core;
using NiL.JS.Core.Interop;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;

namespace Furesoft.LowCode.Nodes.IO.Archives;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.ImportAsObject<Nodes.ScriptInitializer>("Archive");
    }

    [JavaScriptName("archive")]
    public static async void ArchiveDirectory(ArchiveType type, string outputFilename, string path,
        SearchOption searchOption, string searchPattern)
    {
        await using (var zip = File.OpenWrite(outputFilename))
        using (var zipWriter = WriterFactory.Open(zip, type, CompressionType.Deflate))
        {
            zipWriter.WriteAll(path, searchPattern, searchOption);
        }
    }

    [JavaScriptName("extract")]
    public static async void Extract(string archiveFilename, string outputDirectory)
    {
        await using Stream stream = File.OpenRead(archiveFilename);
        var reader = ReaderFactory.Open(stream);

        while (reader.MoveToNextEntry())
        {
            if (reader.Entry.IsDirectory)
            {
                continue;
            }

            Console.WriteLine(reader.Entry.Key);
            reader.WriteEntryToDirectory(outputDirectory, new() {ExtractFullPath = true, Overwrite = true});
        }
    }
}
