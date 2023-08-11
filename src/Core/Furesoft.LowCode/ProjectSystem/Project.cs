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

    public Project Create(string path, string name, string version)
    {
        var proj = new Project {Name = name, Version = version};
        proj.Save(path);

        return proj;
    }
}
