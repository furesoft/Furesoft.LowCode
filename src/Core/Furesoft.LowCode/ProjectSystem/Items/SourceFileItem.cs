namespace Furesoft.LowCode.ProjectSystem.Items;

public class SourceFileItem : ProjectItem
{
    public SourceFileItem(string name, string content) : base(name)
    {
        Content = content;
    }

    public string Content { get; set; }

    public override string ToString()
    {
        return Content;
    }
}
