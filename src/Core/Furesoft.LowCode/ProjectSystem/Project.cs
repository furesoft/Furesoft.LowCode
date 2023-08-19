using System.IO.Compression;
using Newtonsoft.Json;

namespace Furesoft.LowCode.ProjectSystem;

public class Project
{
    private Project()
    {
    }

    public string Name { get; set; }
    public string Version { get; set; }

    [JsonIgnore] public OptionsProvider Options { get; } = new();

    [JsonIgnore] public ObservableCollection<ProjectItem> Items { get; set; } = new();

    public static Project Load(string path)
    {
        using var zip = ZipFile.OpenRead(path);
        using var jsonStream = zip.GetEntry("meta.json")!.Open();
        var optionsEntry = zip.GetEntry("options.json");

        var proj = JsonConvert.DeserializeObject<Project>(new StreamReader(jsonStream).ReadToEnd());

        if (optionsEntry != null)
        {
            using var optionsStream = optionsEntry.Open();
            proj.Options.Open(optionsStream);
        }

        foreach (var entry in zip.Entries)
        {
            if (entry.Name == "meta.json" || entry.Name == "options.json")
            {
                continue;
            }

            var extension = Path.GetExtension(entry.Name);
            var entryStream = entry.Open();
            var sr = new StreamReader(entryStream);
            var entryContent = sr.ReadToEnd();

            ProjectItem item = extension switch
            {
                ".json" => new GraphItem(entry.Name, entryContent),
                ".js" => new SourceFile(entry.Name, entryContent),
                _ => null
            };

            proj.Items.Add(item);
        }

        return proj;
    }

    public void Save(string path)
    {
        using var zip = ZipFile.Open(path, ZipArchiveMode.Create);
        WriteMetadata(zip);
        Options.SaveTo(zip.CreateEntry("options.json").Open());

        foreach (var item in Items)
        {
            CreateEntryWithContent(zip, item.Name, item.Content);
        }
    }

    private static void CreateEntryWithContent(ZipArchive zip, string name, string content)
    {
        var entry = zip.CreateEntry(name).Open();
        using var metaWriter = new StreamWriter(entry);
        metaWriter.Write(content);
    }

    private void WriteMetadata(ZipArchive zip)
    {
        var json = JsonConvert.SerializeObject(this);
        CreateEntryWithContent(zip, "meta.json", json);
    }

    public static Project Create(string name, string version)
    {
        var proj = new Project {Name = name, Version = version};

        return proj;
    }
}
