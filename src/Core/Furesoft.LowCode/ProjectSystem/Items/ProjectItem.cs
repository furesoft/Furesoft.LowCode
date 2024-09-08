using Newtonsoft.Json;

namespace Furesoft.LowCode.ProjectSystem.Items;

public abstract class ProjectItem(string name)
{
    [JsonIgnore] public string Name { get; set; } = name;

    public string Id { get; set; }
}
