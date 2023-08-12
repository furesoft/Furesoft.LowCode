using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Nodes.RPA.Web.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web;

public class NewTabNode : WebNode
{
    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> URL { get; set; }

    public NewTabNode() : base("New Tab")
    {

    }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = await GetBrowser().NewPageAsync();
        await page.GoToAsync(Evaluate(URL));
        
        DefineConstant(PageVariableName, page);
    }
}
