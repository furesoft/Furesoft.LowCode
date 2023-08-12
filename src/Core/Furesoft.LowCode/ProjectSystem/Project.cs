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

    [JsonIgnore] public List<ProjectItem> Items { get; set; } = new();

    public static Project Load(string path)
    {
        using var zip = ZipFile.OpenRead(path);
        using var jsonStream = zip.GetEntry("meta.json")!.Open();
        var proj = JsonConvert.DeserializeObject<Project>(new StreamReader(jsonStream).ReadToEnd());

        foreach (var entry in zip.Entries)
        {
            if (entry.Name == "meta.json")
            {
                continue;
            }

            var extension = Path.GetExtension(entry.Name);
            var entryStream = entry.Open();
            var sr = new StreamReader(entryStream);
            var entryContent = sr.ReadToEnd();

            ProjectItem item = null;
            switch (extension)
            {
                case ".json":
                    item = new Graph(entry.Name, entryContent);
                    break;
                case ".js":
                    item = new SourceFile(entry.Name, entryContent);
                    break;
            }
            
            proj.Items.Add(item);
        }

        return proj;
    }

    public void Save(string path)
    {
        using var zip = ZipFile.Open(path, ZipArchiveMode.Create);
        WriteMetadata(zip);

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
