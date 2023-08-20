using System.ComponentModel.DataAnnotations;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes.IO.Filesystem;

[NodeCategory("IO/FileSystem")]
[Description("Watch for Changes on the filesystem")]
[NodeView(typeof(IconNodeView), "M 9.3 18 H 4 C 2.9 18 2 17.1 2 16 V 4 C 2 2.9 2.9 2 4 2 H 10 L 12 4 H 20 C 21.1 4 22 4.9 22 6 V 12.6 C 20.6 11.6 18.9 11 17 11 C 13.5 11 10.4 13.1 9.1 16.3 L 8.8 17 L 9.1 17.7 C 9.2 17.8 9.2 17.9 9.3 18 M 23 17 C 22.1 19.3 19.7 21 17 21 S 11.9 19.3 11 17 C 11.9 14.7 14.3 13 17 13 S 22.1 14.7 23 17 M 19.5 17 C 19.5 15.6 18.4 14.5 17 14.5 S 14.5 15.6 14.5 17 S 15.6 19.5 17 19.5 S 19.5 18.4 19.5 17 M 17 16 C 16.4 16 16 16.4 16 17 S 16.4 18 17 18 S 18 17.6 18 17 S 17.6 16 17 16")]
public class FileWatcherNode : InputOutputNode
{
    public FileWatcherNode() : base("File Watcher")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string Path { get; set; }

    [DataMember(EmitDefaultValue = false)] public string Filter { get; set; }

    [DataMember(EmitDefaultValue = false)] public bool IncludeSubDirectories { get; set; }

    [Pin("On Change", PinAlignment.Right)] public IOutputPin OnChangedPin { get; set; }

    public NotifyFilters NotifyFilter { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var watcher = new FileSystemWatcher();
        watcher.Path = Path;
        watcher.Filter = Filter;
        watcher.IncludeSubdirectories = IncludeSubDirectories;
        watcher.EnableRaisingEvents = true;
        watcher.NotifyFilter = NotifyFilter;

        watcher.Created += async (sender, args) =>
        {
            await ContinueAfterEvent(cancellationToken, args);
        };
        watcher.Changed += async (sender, args) =>
        {
            await ContinueAfterEvent(cancellationToken, args);
        };
        watcher.Renamed += async (sender, args) =>
        {
            await ContinueAfterEvent(cancellationToken, args);
        };
        watcher.Deleted += async (sender, args) =>
        {
            await ContinueAfterEvent(cancellationToken, args);
        };

        await ContinueWith(OutputPin, cancellationToken);
    }

    private async Task ContinueAfterEvent(CancellationToken cancellationToken, FileSystemEventArgs args)
    {
        DefineConstant("changed", args);
        await ContinueWith(OnChangedPin, cancellationToken);
    }
}
