namespace Furesoft.LowCode.ProjectSystem;

public abstract class ProjectItem
{
    public string Name { get; set; }
    public string Content { get; set; }

    public ProjectItem(string name, string content)
    {
        Name = name;
        Content = content;
    }
}
