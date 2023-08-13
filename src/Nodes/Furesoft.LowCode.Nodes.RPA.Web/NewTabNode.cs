using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.Nodes.RPA.Web.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web;

public class NewTabNode : WebNode
{
    public NewTabNode() : base("New Tab")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> URL { get; set; }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = await GetBrowser().NewPageAsync();
        await page.GoToAsync(URL);

        DefineConstant(PageVariableName, page);
    }
}
