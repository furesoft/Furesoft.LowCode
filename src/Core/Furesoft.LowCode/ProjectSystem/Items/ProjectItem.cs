namespace Furesoft.LowCode.ProjectSystem.Items;

public abstract class ProjectItem
{
    public ProjectItem(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public string Id { get; set; }
}
