using Newtonsoft.Json;

namespace Furesoft.LowCode.ProjectSystem.Items;

public abstract class ProjectItem
{
    public ProjectItem(string name)
    {
        Name = name;
    }

    [JsonIgnore] public string Name { get; set; }

    public string Id { get; set; }
}
