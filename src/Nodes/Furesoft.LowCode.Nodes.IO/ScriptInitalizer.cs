using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.Nodes.IO.Filesystem;
using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace Furesoft.LowCode.Nodes.IO;

public class ScriptInitalizer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.DefineConstructor(typeof(ItemType));

        context.ImportAsObject<ScriptInitalizer>("FS");
    }

    [JavaScriptName("childItem")]
    public static Result ChildItem(bool isRecurse, string searchPattern, string folderPath, bool followSymlink, FileAttributes excludedFlags, ItemType itemType)
    {
        var searchOption = isRecurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

        if (string.IsNullOrEmpty(searchPattern))
        {
            searchPattern = "*";
        }

        var dirInfo = new DirectoryInfo(folderPath);

        if (followSymlink)
        {
            if (dirInfo.ResolveLinkTarget(true) is DirectoryInfo resolvedDir)
            {
                dirInfo = resolvedDir;
            }
            else
            {
                return Result.Failure<IOException>("Resolved Symlink doesn't link to a Directory");
            }
        }

        var fileInfos = itemType switch
        {
            ItemType.File => dirInfo.GetFiles(searchPattern, searchOption),
            ItemType.Directory => dirInfo.GetDirectories(searchPattern, searchOption),
            ItemType.All => dirInfo.GetFileSystemInfos(searchPattern, searchOption),
            _ => null
        };

        if (fileInfos == null)
        {
            return Result.Failure<InvalidOperationException>("Unknown Type");
        }

        return Result.Ok(fileInfos.Where(x => x.Attributes.HasFlag(excludedFlags)));
    }
}
