using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.Nodes.RPA.Web.Core;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes.RPA.Web.Interaction;

[NodeView(typeof(IconNodeView),
    Parameter =
        "M10 3H9V2h1v1zM3 4H2v1h1V4zm5-2H7v1h1V2zM4 2H2v1h2V2zm8 7h2v-1h-2v1zM8 5h1V4H8v1zm-4 3H2v1h2v-1zm8-6h-1v1h1V2zm2 0h-1v1h1V2zm-2 5h2V4h-2v3zm4-6v9c0 .55-.45 1-1 1H1c-.55 0-1-.45-1-1V1c0-.55.45-1 1-1h14c.55 0 1 .45 1 1zm-1 0H1v9h14V1zM6 5h1V4H6v1zm0-3H5v1h1V2zM4 5h1V4H4v1zm1 4h6v-1H5v1zm5-4h1V4h-1v1zM3 6H2v1h1V6zm5 0v1h1V6H8zM6 6v1h1V6H6zM5 6H4v1h1V6zm5 1h1V6h-1v1z")]
public class TypeIntoNode : WebNode
{
    public TypeIntoNode() : base("Type Into")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string Selector { get; set; } = string.Empty;


    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> Text { get; set; }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = GetPage();

        var element = await page.QuerySelectorAsync(Selector);

        await element.TypeAsync(Text);
    }
}
