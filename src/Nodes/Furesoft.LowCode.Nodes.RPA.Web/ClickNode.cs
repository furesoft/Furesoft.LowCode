namespace Furesoft.LowCode.Nodes.RPA.Web;

public class ClickNode : WebNode
{
    public string Selector { get; set; } = string.Empty;

    public ClickNode() : base("Click")
    {
    }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = GetPage();
        
        var element = await page.QuerySelectorAsync(Selector);
        
        await element.ClickAsync();
    }
}
